using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using XavierPlayLandAPI.Models.Repositories;
using XavierPlayLandAPI.Models;
using XavierPlayLandAPI.Filters.ActionFilters;
using XavierPlayLandAPI.Filters;
using XavierPlayLandAPI.Filters.ExceptionFilters;

namespace XavierPlayLandAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderRepository.GetAllOrders();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        [ValidateEntityIdFilter(EntityType.Order)]
        public IActionResult GetOrder(int id)
        {
            var order = _orderRepository.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(order);
        }

        [HttpPost]
        [ValidateAddEntityFilter(EntityType.Order)]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // The request data did not pass validation checks
                    return BadRequest(ModelState);
                }

                await _orderRepository.CreateOrder(order);
                return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [ValidateEntityIdFilter(EntityType.Order)]
        [ValidateUpdateEntityFilter(EntityType.Order)]
        [HandleUpdateExceptionsFilter]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] Order order)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    // The request data did not pass validation checks
                    return BadRequest(ModelState);
                }

                if (id != order.Id)
                {
                    return BadRequest("Order ID mismatch.");
                }

                await _orderRepository.UpdateOrder(order);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ValidateEntityIdFilter(EntityType.Order)]
        public IActionResult DeleteOrder(int id)
        {
            var existingOrder = _orderRepository.GetOrderById(id);
            if (existingOrder == null)
            {
                return NotFound();
            }

            _orderRepository.DeleteOrder(id);
            return NoContent();
        }
    }
}
