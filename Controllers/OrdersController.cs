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
    public class OrdersController : ControllerBase
    {
        public IOrderService _orderService;
        public IStockService _stockService;

        public IMapper _mapper { get; }

        public OrdersController(IOrderService orderService, IMapper mapper, IStockService stockService)
        {
            _orderService = orderService;
            _mapper = mapper;
            _stockService = stockService;
        }

        [HttpGet]
        public async Task<ActionResult<List<VMGetOrder>>> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrders();

            var mappedOrders = _mapper.Map<List<VMGetOrder>>(orders);
            if (orders == null)
            {
                return NotFound(new Response(MESSAGE.DATA_NOT_FOUND, false));
            }

            return Ok(new Response<List<VMGetOrder>>(mappedOrders, true, MESSAGE.LOADED));
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<VMGetOrder>> GetOrderById(int Id)
        {
            var order = await _orderService.GetByIdAsync(Id);
            var mappoedOrder = _mapper.Map<VMGetOrder>(order);

            if (order == null)
            {
                return NotFound(new Response<IEnumerable<VMGetOrder>>(MESSAGE.DATA_NOT_FOUND, false));
            }

            return Ok(new Response<VMGetOrder>(mappoedOrder, true, MESSAGE.LOADED));
        }

        [HttpPost]
        public async Task<ActionResult<VMGetOrder>> AddOrder([FromBody] VMCreateOrder vMCreateOrder)
        {
            // Fetch the stock by Sid
            var stock = await _stockService.GetByIdAsync(vMCreateOrder.Sid);

            if (stock == null)
            {
                return NotFound(new Response<VMGetOrder>("Stock not found!", false));
            }

            // Check if the order quantity is greater than the available stock quantity
            if (vMCreateOrder.Quantity <= 0 || vMCreateOrder.Quantity > stock.Quantity)
            {
                return BadRequest(new Response<VMGetOrder>("Order quantity is invalid or exceeds available stock!", false));
            }

            var orderToBeAdded = new Order
            {
                CustomerName = vMCreateOrder.CustomerName,
                Sid = (int)vMCreateOrder.Sid,
                Quantity = vMCreateOrder.Quantity
            };

            var addedOrder = await _orderService.AddAsync(orderToBeAdded);

            // Update the stock quantity
            stock.Quantity -= vMCreateOrder.Quantity;
            await _stockService.UpdateAsync(stock);

            var mappedOrder = _mapper.Map<VMGetOrder>(addedOrder);

            return CreatedAtAction(nameof(GetOrderById), new { id = addedOrder.Id }, new Response<VMGetOrder>(mappedOrder, true, MESSAGE.SAVED));
        }


        [HttpDelete("{Id}")]
        public async Task<IActionResult> DeleteOrder(int Id)
        {
            var order = await _orderService.GetByIdAsync(Id);
            if (order == null)
            {
                return NotFound(new Response<object>(MESSAGE.DATA_NOT_FOUND, false));
            }

            var stock = await _stockService.GetByIdAsync(order.Sid);
            if (stock == null)
            {
                return NotFound(new Response<object>("Related stock not found!", false));
            }

            stock.Quantity += order.Quantity;
            await _stockService.UpdateAsync(stock);

            await _orderService.Remove(order);

            var mappedOrder = _mapper.Map<VMGetOrder>(order);

            return Ok(new Response<VMGetOrder>(mappedOrder, true, MESSAGE.DELETED));
        }


    }
}
