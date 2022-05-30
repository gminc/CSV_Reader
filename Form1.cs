using CsvHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Threading.Tasks;

namespace ReadCSV
{
    public partial class Form1 : Form
    {
        private GroupStorage<string, StockData> groupByStock { get; set; }
        private string[] AllStockId { get; set; }
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 讀取檔案按鈕
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReadFielButton_Click(object sender, EventArgs e)
        {
            string file;
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Title = "請選擇檔案";
            ReadFileState.Text = "讀檔中";
            openFile.Filter = "CSV檔案(.csv)|*.csv*";
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                file = openFile.FileName;
                FilePathTextBox.Text = file;
                StockSelectComboBox.Text = "All";
                Timer.Start();
            }
            else
            {
                return;
            }
            StockData row;
            groupByStock = new GroupStorage<string, StockData>(1, 1);
            using (var reader = new StreamReader(file))
            {
                using (var csv = new CsvReader(reader, CultureInfo.CurrentCulture))
                {
                    while (csv.Read())
                    {
                        row = csv.GetRecord<StockData>();
                        groupByStock.TryAdd(row.StockId, row);
                    }
                }
            }
            StockDataGridView.DataSource = groupByStock.ToList();
            string readFileTime = Timer.Stop();
            ReadFileState.Text = "讀檔完成";

            Timer.Start();
            StockSelectComboBox.Items.Add("All");
            Tuple<string[], string[]> result = groupByStock.GetAllStockIdAndComboBox();            
            StockSelectComboBox.Items.AddRange(result.Item1);
            AllStockId = result.Item2;
            string ComboBoxTime = Timer.Stop();

            CostTimeTextBox.Text = "讀取時間: " + readFileTime +
                "ComboBox產生時間: " + ComboBoxTime;
        }

        /// <summary>
        /// 查詢指定股票
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchStockButton_Click(object sender, EventArgs e)
        {
            if (groupByStock == null)
            {
                return;
            }
            Timer.Start();
            string[] searchTarget = SearchContent(StockSelectComboBox.Text);
            List<StatisticsData> statistics = Statistics(searchTarget);
            List<StockData> searchStockDatas = new List<StockData>();
            
            foreach (string id in searchTarget)
            {
                groupByStock.TryFindByKey(id, out List<StockData> result);
                searchStockDatas.AddRange(result);
            }
            StockDataGridView.DataSource = searchStockDatas;
            StatisticsDataGridView.DataSource = statistics;
            CostTimeTextBox.Text += "查詢時間: " + Timer.Stop();
        }

        /// <summary>
        /// 查詢買賣超Top50
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Top50Button_Click(object sender, EventArgs e)
        {
            if (groupByStock == null)
            {
                return;
            }
            Timer.Start();
            List<TopStockData> top50 = StatisticsTopData(SearchContent(StockSelectComboBox.Text), 50);
            Top50DataGridView.DataSource = top50;
            CostTimeTextBox.Text += "Top50 產生時間: " + Timer.Stop();
        }

        /// <summary>
        /// 處理輸入的查詢字串
        /// </summary>
        /// <param name="searchTarget"></param>
        /// <returns></returns>
        private string[] SearchContent(string searchTarget)
        {
            string[] stockIds = null;
            if (searchTarget.Contains(" - "))
            {
                stockIds = searchTarget.Split('-');
                return new string[] { stockIds[0].Replace(" ", string.Empty) };
            }
            else if (searchTarget.Contains(","))
            {
                return searchTarget.Split(',');
            }
            else if (searchTarget.Equals("All"))
            {
                return AllStockId;
            }
            else
            {
                return new string[1] { searchTarget };
            }
        }

        /// <summary>
        /// 統計股票資料
        /// </summary>
        /// <param name="stockIds"></param>
        /// <returns></returns>
        private List<StatisticsData> Statistics(string[] stockIds)
        {
            groupByStock.TryGetStatisticsValue(stockIds, out List<StatisticsData> result);
            return result;
        }

        /// <summary>
        /// 統計前selectNum筆資料資料
        /// </summary>
        /// <param name="stockIds"></param>
        /// <param name="selectNum"></param>
        /// <returns></returns>
        private List<TopStockData> StatisticsTopData(string[] stockIds, int selectNum)
        {
            List<TopStockData> result = new List<TopStockData>();
            SortedList<int, List<TopStockData>> buySellOverSort;
            StockData rowData;
            for (int i = 0; i < stockIds.Count(); ++i)
            {
                groupByStock.TryFindByKey(stockIds[i], out List<StockData> stock);
                buySellOverSort = new SortedList<int, List<TopStockData>>();
                foreach (IGrouping<string, StockData> secBroker in stock.GroupBy(data => data.SecBrokerId))
                {
                    int buySellOver = 0;
                    foreach (StockData stockData in secBroker)
                    {
                        buySellOver += stockData.BuyQty - stockData.SellQty;
                    }
                    if (!buySellOverSort.ContainsKey(buySellOver))
                    {
                        buySellOverSort.Add(buySellOver, new List<TopStockData>());
                    }
                    rowData = secBroker.First();
                    buySellOverSort[buySellOver].Add(new TopStockData()
                    {
                        StockNmae = rowData.StockName,
                        SecBrokerName = rowData.SecBrokerName,
                        BuySellOver = buySellOver
                    });
                }
                result.AddRange(GetTopNum(buySellOverSort, selectNum, GetDataCondition.Top));
                result.AddRange(GetTopNum(buySellOverSort, selectNum, GetDataCondition.End));
            }
            return result;
        }

        /// <summary>
        /// 取得資料設定
        /// </summary>
        private enum GetDataCondition { Top, End }

        /// <summary>
        /// 取得前N筆資料
        /// </summary>
        /// <param name="takeNum"></param>
        /// <returns></returns>
        private List<TopStockData> GetTopNum(SortedList<int, List<TopStockData>> source, int takeNum, GetDataCondition condition)
        {
            if (source.Count < takeNum)
            {
                takeNum = source.Count;
            }
            List<TopStockData> result = new List<TopStockData>(takeNum);
            int count = 0;
            bool isDone = false;
            switch (condition)
            {
                case GetDataCondition.Top:
                    for (int i = source.Keys.Count - 1; i >= 0; --i)
                    {
                        int key = source.Keys[i];
                        if (key <= 0)
                        {
                            break;
                        }
                        foreach (TopStockData stock in source[key])
                        {
                            result.Add(stock);
                            count++;
                            if (count == takeNum)
                            {
                                isDone = true;
                                break;
                            }
                        }
                        if (isDone)
                        {
                            break;
                        }
                    }
                    break;
                case GetDataCondition.End:
                    foreach (int key in source.Keys)
                    {
                        if (key >= 0)
                        {
                            break;
                        }
                        foreach (TopStockData stock in source[key])
                        {
                            result.Add(stock);
                            count++;
                            if (count == takeNum)
                            {
                                isDone = true;
                                break;
                            }
                        }
                        if (isDone)
                        {
                            break;
                        }
                    }
                    break;
            }
            return result;
        }

        /// <summary>
        /// 計時器
        /// </summary>
        internal static class Timer
        {
            private static readonly Stopwatch StopWatch = new Stopwatch();
            /// <summary>
            /// 開始計時
            /// </summary>
            public static void Start()
            {
                StopWatch.Reset();
                StopWatch.Start();
            }
            /// <summary>
            /// 結束計時
            /// </summary>
            /// <returns></returns>
            public static string Stop()
            {
                StopWatch.Stop();
                TimeSpan timeSpan = StopWatch.Elapsed;
                return string.Format("{0:00}:{1:00}:{2:00}:{3:000}{4}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds, Environment.NewLine);
            }
        }

    }
}
