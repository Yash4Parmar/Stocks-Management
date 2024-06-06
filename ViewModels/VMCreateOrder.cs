namespace Stocks_Management.ViewModels
{
    public class VMCreateOrder
    {
        public string CustomerName { get; set; } = null!;
        public int Quantity { get; set; }
        public int Sid { get; set; }
    }
}
