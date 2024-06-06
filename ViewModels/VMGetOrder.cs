namespace Stocks_Management.ViewModels
{
    public class VMGetOrder
    {
        public int Id { get; set; }

        public string CustomerName { get; set; } = null!;

        public int Quantity { get; set; }

        public int Sid { get; set; }

        public string StockName { get; set; } = string.Empty;
    }
}
