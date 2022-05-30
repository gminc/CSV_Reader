using CsvHelper.Configuration.Attributes;

namespace ReadCSV
{
    class StockData
    {
        [Index(0)]
        public string DealDate { get; set; }
        [Index(1)]
        public string StockId { get; set; }
        [Index(2)]
        public string StockName { get; set; }
        [Index(3)]
        public string SecBrokerId { get; set; }
        [Index(4)]
        public string SecBrokerName { get; set; }
        [Index(5)]
        public decimal Price { get; set; }
        [Index(6)]
        public int BuyQty { get; set; }
        [Index(7)]
        public int SellQty { get; set; }
    }
}
