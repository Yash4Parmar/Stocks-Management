using Microsoft.EntityFrameworkCore;
using Stocks_Management.Configuration;
using Stocks_Management.Controllers;
using Stocks_Management.Middlewares;
using Stocks_Management.Models;

namespace Stocks_Management
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddRepositories();
            builder.Services.AddRepoServices();
            builder.Services.AddControllers();
            builder.Services.AddDbContext<StockManagementContext>(
                options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));


            builder.Services.AddTransient<ExceptionMiddelware>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //Exception middelware
            app.ConfigureExceptionMiddelware();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
