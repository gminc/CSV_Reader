using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadCSV
{
    /// <summary>
    /// 依照Key分類儲存List<Value>
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    class GroupStorage<TKey, TValue> : IEnumerable<TValue>
    {
        internal KeyValuePair<TKey, List<TValue>>[][] Buckets { get; set; }
        private int BucketCollisionRresizeThreshold { get; set; }
        public int ValueCount { get; private set; }
        public int KeyCount { get; private set; }
        private int CollisionArrIniSize { get; set; }
        internal int[] BucketsCollisionCount { get; set; }

        /// <summary>
        /// 建構子設定初始分類桶數量和分類桶擴充門檻
        /// </summary>
        /// <param name="iniBucketCapacity"></param>
        /// <param name="bucketCollisionRresizeThreshold"></param>
        public GroupStorage(int iniBucketCapacity, int bucketCollisionRresizeThreshold)
        {
            iniBucketCapacity = iniBucketCapacity < 0 ? 1 : iniBucketCapacity;
            bucketCollisionRresizeThreshold = bucketCollisionRresizeThreshold < 4 ? 4 : bucketCollisionRresizeThreshold;
            Buckets = new KeyValuePair<TKey, List<TValue>>[iniBucketCapacity][];
            BucketCollisionRresizeThreshold = bucketCollisionRresizeThreshold;
            CollisionArrIniSize = bucketCollisionRresizeThreshold > 4 ? 4 : bucketCollisionRresizeThreshold;
            BucketsCollisionCount = new int[iniBucketCapacity];
        }

        /// <summary>
        /// 批量加入value的list
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newData"></param>
        public void AddList(TKey key, List<TValue> newData)
        {
            int index = Math.Abs(key.GetHashCode()) % Buckets.Length;
            BucketCollisionArrCheck(index);
            if (IsContainsKey(key))
            {
                FindByKey(key).AddRange(newData);
            }
            else
            {
                Buckets[index][BucketsCollisionCount[index]++] = new KeyValuePair<TKey, List<TValue>>(key, newData);
                CheckBucketSize(index);
                KeyCount++;
            }
            ValueCount += newData.Count;
        }

        /// <summary>
        /// value逐筆加入
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newData"></param>
        public void Add(TKey key, TValue newData)
        {
            int index = Math.Abs(key.GetHashCode()) % Buckets.Length;
            BucketCollisionArrCheck(index);
            if (!IsContainsKey(key))
            {
                Buckets[index][BucketsCollisionCount[index]++] = new KeyValuePair<TKey, List<TValue>>(key, new List<TValue>());
                CheckBucketSize(index);
                KeyCount++;
            }
            FindByKey(key).Add(newData);
            ValueCount++;
        }

        /// <summary>
        /// 尋找是否有Key的同時添加資料
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newData"></param>
        public void TryAdd(TKey key, TValue newData)
        {
            int index = Math.Abs(key.GetHashCode()) % Buckets.Length;
            BucketCollisionArrCheck(index);
            foreach (KeyValuePair<TKey, List<TValue>> data in Buckets[index])
            {
                if (key.Equals(data.Key))
                {
                    data.Value.Add(newData);
                    ValueCount++;
                    return;
                }
            }
            Buckets[index][BucketsCollisionCount[index]++] = new KeyValuePair<TKey, List<TValue>>(key, new List<TValue>());
            CheckBucketSize(index);
            KeyCount++;
            FindByKey(key).Add(newData);
            ValueCount++;
        }

        /// <summary>
        /// 檢查bucket碰撞儲存arr是否需要擴充
        /// </summary>
        /// <param name="index"></param>
        private void BucketCollisionArrCheck(int index)
        {
            if (Buckets[index] == null)
            {
                Buckets[index] = new KeyValuePair<TKey, List<TValue>>[CollisionArrIniSize];
            }
            if (Buckets[index].Length == BucketsCollisionCount[index])
            {
                Buckets[index] = DoubleBucketCollisionArr(Buckets[index]);
            }
        }

        /// <summary>
        /// 檢查Bucket是否需要擴充
        /// </summary>
        /// <param name="index"></param>
        private void CheckBucketSize(int index)
        {
            if (BucketsCollisionCount[index] == BucketCollisionRresizeThreshold)
            {
                DoubleBucketSize();
            }
        }

        /// <summary>
        /// 取得key對應的list
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<TValue> FindByKey(TKey key)
        {
            int index = Math.Abs(key.GetHashCode()) % Buckets.Length;
            foreach (KeyValuePair<TKey, List<TValue>> data in Buckets[index])
            {
                if (data.Key.Equals(key))
                {
                    return data.Value;
                }
            }
            return default(List<TValue>);
        }

        /// <summary>
        /// 避免報錯
        /// </summary>
        /// <param name="key"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TryFindByKey(TKey key, out List<TValue> result)
        {
            int index = Math.Abs(key.GetHashCode()) % Buckets.Length;
            if (Buckets[index] == null)
            {
                result = new List<TValue>();
                return false;
            }
            foreach (KeyValuePair<TKey, List<TValue>> data in Buckets[index])
            {
                if (data.Key.Equals(key))
                {
                    result = data.Value;
                    return true;
                }
            }
            result = new List<TValue>();
            return false;
        }

        /// <summary>
        /// Buckets擴充
        /// </summary>
        private void DoubleBucketSize()
        {
            int newLength = Buckets.Length * 2;
            KeyValuePair<TKey, List<TValue>>[][] newBucket = new KeyValuePair<TKey, List<TValue>>[newLength][];
            BucketsCollisionCount = new int[newLength];
            foreach (KeyValuePair<TKey, List<TValue>>[] bucket in Buckets)
            {
                if (bucket == null)
                {
                    continue;
                }
                foreach (KeyValuePair<TKey, List<TValue>> data in bucket)
                {
                    if (data.Key == null)
                    {
                        break;
                    }
                    int index = Math.Abs(data.Key.GetHashCode()) % newBucket.Length;
                    if (newBucket[index] == null)
                    {
                        newBucket[index] = new KeyValuePair<TKey, List<TValue>>[bucket.Length];
                    }
                    newBucket[index][BucketsCollisionCount[index]++] = data;
                }
            }
            Buckets = newBucket;
        }

        /// <summary>
        /// BucketCollisionArr擴充
        /// </summary>
        private KeyValuePair<TKey, List<TValue>>[] DoubleBucketCollisionArr(KeyValuePair<TKey, List<TValue>>[] oldArr)
        {
            KeyValuePair<TKey, List<TValue>>[] newArr = new KeyValuePair<TKey, List<TValue>>[oldArr.Length * 2];
            int count = 0;
            foreach (KeyValuePair<TKey, List<TValue>> data in oldArr)
            {
                newArr[count++] = data;
            }
            return newArr;
        }

        /// <summary>
        /// key是否已加入
        /// </summary>
        /// <returns></returns>
        public bool IsContainsKey(TKey key)
        {
            int index = Math.Abs(key.GetHashCode()) % Buckets.Length;
            if (Buckets[index] == null)
            {
                return false;
            }
            foreach (KeyValuePair<TKey, List<TValue>> data in Buckets[index])
            {
                if (key.Equals(data.Key))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 試著取出指定的統計股票資料
        /// </summary>
        /// <param name="stockIds"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public bool TryGetStatisticsValue(string[] stockIds, out List<StatisticsData> result)
        {
            bool hasValue = false;
            result = new List<StatisticsData>();
            for (int i = 0; i < stockIds.Count(); ++i)
            {
                int index = Math.Abs(stockIds[i].GetHashCode()) % Buckets.Length;
                if (Buckets[index] == null)
                {
                    hasValue = false;
                    continue;
                }
                foreach (KeyValuePair<TKey, List<TValue>> data in Buckets[index])
                {
                    if (stockIds[i].Equals(data.Key))
                    {
                        List<StockData> stock = (List<StockData>)Convert.ChangeType(data.Value, typeof(List<StockData>));
                        int buyTotal = stock.Sum(number => number.BuyQty);
                        int sellTotal = stock.Sum(number => number.SellQty);
                        result.Add(new StatisticsData()
                        {
                            StockId = stockIds[i].ToString(),
                            StockName = stock[0].StockName,
                            BuyTotal = buyTotal,
                            SellTotal = sellTotal,
                            AvgPrice = stock.Average(number => number.Price),
                            BuySellOver = buyTotal - sellTotal,
                            SecBrokerCnt = stock.Select(number => number.SecBrokerId).Distinct().Count()
                        });
                        hasValue = true;
                    }
                }
                hasValue = false;
            };
            return hasValue;
        }

        /// <summary>
        /// 取得key的list
        /// </summary>
        /// <returns></returns>
        public List<TKey> GetKeyList()
        {
            List<TKey> result = new List<TKey>(KeyCount);
            foreach (var bucket in Buckets)
            {
                if (bucket == null)
                {
                    continue;
                }
                foreach (var data in bucket)
                {
                    if (data.Key == null)
                    {
                        break;
                    }
                    result.Add(data.Key);
                }
            }
            return result;
        }

        /// <summary>
        /// 取得 AllStockId 和 ComboBox 的內容
        /// </summary>
        /// <returns></returns>
        public Tuple<string[], string[]> GetAllStockIdAndComboBox()
        {
            string[] stocks = new string[KeyCount];
            string[] AllStockId = new string[KeyCount];
            int count = 0;
            foreach (var bucket in Buckets)
            {
                if (bucket == null)
                {
                    continue;
                }
                foreach (var data in bucket)
                {
                    if (data.Key == null)
                    {
                        break;
                    }
                    AllStockId[count] = data.Key.ToString();
                    StockData stockData = (StockData)Convert.ChangeType(data.Value[0], typeof(StockData));
                    stocks[count++] = data.Key.ToString() + " - " + stockData.StockName;
                }
            }
            return new Tuple<string[], string[]>(stocks, AllStockId);
        }

        /// <summary>
        /// 取得迭代器
        /// </summary>
        /// <returns></returns>
        public IEnumerator<TValue> GetEnumerator()
        {
            return new LiteDictEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// 迭代器
        /// </summary>
        internal class LiteDictEnumerator : IEnumerator<TValue>
        {
            private GroupStorage<TKey, TValue> LiteDict { get; set; }
            private int BucketIndex { get; set; }
            private int CollisionArrIndex { get; set; }
            private int ValueIndex { get; set; }

            internal LiteDictEnumerator(GroupStorage<TKey, TValue> liteDict)
            {
                LiteDict = liteDict;
                Reset();
            }

            /// <summary>
            /// 取得當前指標上的物件
            /// </summary>
            public TValue Current => LiteDict.Buckets[BucketIndex][CollisionArrIndex].Value[ValueIndex];
            object IEnumerator.Current => this.Current;

            /// <summary>
            /// 移至下一物件
            /// </summary>
            /// <returns></returns>
            public bool MoveNext()
            {
                while (LiteDict.Buckets[BucketIndex] == null)
                {
                    BucketIndex++;
                    CollisionArrIndex = 0;
                    ValueIndex = 0;
                }
                if (ValueIndex++ != LiteDict.Buckets[BucketIndex][CollisionArrIndex].Value.Count - 1)
                {
                    return true;
                }
                ValueIndex = 0;
                if (CollisionArrIndex++ != LiteDict.BucketsCollisionCount[BucketIndex] - 1)
                {
                    return true;
                }
                CollisionArrIndex = 0;
                do
                {
                    if (++BucketIndex == LiteDict.Buckets.Length)
                    {
                        Reset();
                        return false;
                    }
                } while (LiteDict.Buckets[BucketIndex] == null);
                return true;
            }

            /// <summary>
            /// 回到迭代起點
            /// </summary>
            public void Reset()
            {
                BucketIndex = 0;
                CollisionArrIndex = 0;
                ValueIndex = 0;
            }

            /// <summary>
            /// 釋放資源
            /// </summary>
            public void Dispose()
            {
                LiteDict = null;
            }

        }
    }
}
