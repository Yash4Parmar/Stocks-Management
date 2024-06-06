using Stocks_Management.Repository;
using Stocks_Management.Interfaces;
using Stocks_Management.Models;
using Stocks_Management.ViewModels;

namespace Stocks_Management.Services
{
    public class StockService : IStockService
    {
        public IDBRepository<Stock> _stockRepo { get; }

        public StockService(IDBRepository<Stock> stockRepo)
        {
            _stockRepo = stockRepo;
        }
        public async Task<Stock> AddAsync(Stock stock)
        {
            return await _stockRepo.AddAsync(stock);
        }
        public async Task<List<Stock>> GetAllStocks()
        {
            var allStocks = await _stockRepo.GetAllAsync();
            var stocks = allStocks.Where(s => s.IsDeleted == null || s.IsDeleted == false);
            return new List<Stock>(stocks);
        }

        public async Task<Stock> GetByIdAsync(int Id)
        {
            return await _stockRepo.GetByIdAsync(Id);
        }

        public Task<Stock> Remove(Stock stock)
        {
            return _stockRepo.Remove(stock);

        }
    }
}
