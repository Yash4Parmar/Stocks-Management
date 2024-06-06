using Stocks_Management.Repository;
using Stocks_Management.Interfaces;
using Stocks_Management.Models;
using Stocks_Management.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Stocks_Management.Services
{
    public class OrderService : IOrderService
    {
        private readonly StockManagementContext _context;

        public IDBRepository<Order> _orderRepo { get; }

        public OrderService(IDBRepository<Order> orderRepo, StockManagementContext context)
        {
            _orderRepo = orderRepo;
            _context = context;
        }
        public async Task<Order> AddAsync(Order order)
        {
            return await _orderRepo.AddAsync(order);
        }
        public async Task<List<VMGetOrder>> GetAllOrders()
        {
            //var allOrders = await _orderRepo.GetAllAsync();
            var orders = await (from order in _context.Orders
                                join stock in _context.Stocks on order.Sid equals stock.Id
                                where order.IsDeleted == null || order.IsDeleted == false
                                select new VMGetOrder
                                {
                                    Id = order.Id,
                                    CustomerName = order.CustomerName,
                                    Quantity = order.Quantity,
                                    Sid = order.Sid,
                                    StockName = stock.Name
                                }).ToListAsync();

            return new List<VMGetOrder>(orders);
        }

        public async Task<Order> GetByIdAsync(int Id)
        {
            return await _orderRepo.GetByIdAsync(Id);
        }

        public Task<Order> Remove(Order order)
        {
            return _orderRepo.Remove(order);

        }
    }
}
