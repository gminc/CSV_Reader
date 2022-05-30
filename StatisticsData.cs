using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadCSV
{
    class StatisticsData
    {
        public string StockId { get; set; }
        public string StockName { get; set; }
        public int BuyTotal { get; set; }
        public int SellTotal { get; set; }
        public decimal AvgPrice { get; set; }
        public int BuySellOver { get; set; }
        public int SecBrokerCnt { get; set; }
    }
}
