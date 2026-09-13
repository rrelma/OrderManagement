using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;

namespace Api.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ICreateOrderCommandHandler _createOrderHandler;
        private readonly IGetOrderByIdQueryHandler _getOrderByIdHandler;

        public OrdersController(
                ICreateOrderCommandHandler createOrderHandler,
                IGetOrderByIdQueryHandler getOrderByIdHandler)
        {
            _createOrderHandler = createOrderHandler;
            _getOrderByIdHandler = getOrderByIdHandler;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command, CancellationToken ct)
        {
            var orderId = await _createOrderHandler.HandleAsync(command, ct);
            return Ok(new { OrderId = orderId });
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetOrderById(Guid id, CancellationToken ct)
        {
            var query = new GetOrderByIdQuery(id);
            var order = await _getOrderByIdHandler.HandleAsync(query, ct);

            if (order == null) return NotFound();

            return Ok(order);
        }
    }
}
