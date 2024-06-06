using Stocks_Management.Models;
using Stocks_Management.ViewModels;

namespace Stocks_Management.Interfaces
{
    public interface IOrderService
    {
        Task<List<VMGetOrder>> GetAllOrders();
        Task<Order> GetByIdAsync(int id);
        Task<Order> AddAsync(Order order);
        Task<Order> Remove(Order order);

    }
}
