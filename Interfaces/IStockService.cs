using Stocks_Management.Models;
using Stocks_Management.ViewModels;

namespace Stocks_Management.Interfaces
{
    public interface IStockService
    {
        Task<List<Stock>> GetAllStocks();
        Task<Stock> GetByIdAsync(int id);
        Task<Stock> AddAsync(Stock stock);

        Task<Stock> Remove(Stock stock);

    }
}
