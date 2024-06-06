using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using Stocks_Management.Interfaces;
using Stocks_Management.Models;
using Stocks_Management.ViewModels;

namespace Stocks_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        public IStockService _stockService;
        public IMapper _mapper { get; }

        public StocksController(IStockService stockService, IMapper mapper)
        {
            _stockService = stockService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<VMGetStock>>> GetAllStocks()
        {
            var stocks = await _stockService.GetAllStocks();

            var mappedStocks = _mapper.Map<List<VMGetStock>>(stocks);
            if (stocks == null)
            {
                return NotFound(new Response(MESSAGE.DATA_NOT_FOUND, false));
            }

            return Ok(new Response<List<VMGetStock>>(mappedStocks, true, MESSAGE.LOADED));
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<VMGetStock>> GetStockById(int Id)
        {
            var stock = await _stockService.GetByIdAsync(Id);
            var mappoedStock = _mapper.Map<VMGetStock>(stock);

            if (stock == null)
            {
                return NotFound(new Response<IEnumerable<VMGetStock>>(MESSAGE.DATA_NOT_FOUND, false));
            }

            return Ok(new Response<VMGetStock>(mappoedStock, true, MESSAGE.LOADED));
        }

        [HttpPost]
        public async Task<ActionResult<VMGetStock>> AddHoliday([FromBody] VMCreateStock vMCreateStock)
        {
            var stockToBeAdded = new Stock
            {
                Name = vMCreateStock.Name,
                Quantity = vMCreateStock.Quantity
            };

            if (stockToBeAdded.Quantity <= 0)
            {
                return Ok(new Response<VMGetStock>("Stock quantity can't be 0 or negative value!", false));
            }
            var stock = _mapper.Map<Stock>(stockToBeAdded);
            var addedStock = await _stockService.AddAsync(stock);
            var mappedStock = _mapper.Map<VMGetStock>(stockToBeAdded);

            return CreatedAtAction(nameof(GetStockById), new { id = addedStock.Id }, new Response<VMGetStock>(mappedStock, true, MESSAGE.SAVED));
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteStock(int Id)
        {
            var stock = await _stockService.GetByIdAsync(Id);
            if (stock == null)
            {
                return NotFound(new Response<object>(MESSAGE.DATA_NOT_FOUND, false));
            }

            _stockService?.Remove(stock);

            var mappedStock = _mapper.Map<VMGetStock>(stock);

            return CreatedAtAction(nameof(GetStockById), new { Id = mappedStock.Id }, new Response<VMGetStock>(mappedStock, true, MESSAGE.DELETED));
        }

    }
}
