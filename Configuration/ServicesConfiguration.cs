using Stocks_Management.Repository;
using Stocks_Management.Interfaces;
using Stocks_Management.Services;

namespace Stocks_Management.Configuration
{
    public static class ServicesConfiguration
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient(typeof(IDBRepository<>), typeof(DBRepository<>)); // Register DBRepository<TEntity>
        }
        public static void AddRepoServices(this IServiceCollection services)
        {
            services.AddScoped<IStockService, StockService>();
            services.AddScoped<IOrderService, OrderService>();
        }
    }
}