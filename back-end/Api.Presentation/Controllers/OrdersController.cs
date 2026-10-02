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
        private readonly IGetAllOrdersQueryHandler _getAllOrdersHandler;
        private readonly IShipOrderCommandHandler _shipOrderHandler;

        public OrdersController(
                ICreateOrderCommandHandler createOrderHandler,
                IGetOrderByIdQueryHandler getOrderByIdHandler,
                IGetAllOrdersQueryHandler getAllOrdersHandler,
                IShipOrderCommandHandler shipOrderHandler)
        {
            _createOrderHandler = createOrderHandler;
            _getOrderByIdHandler = getOrderByIdHandler;
            _getAllOrdersHandler = getAllOrdersHandler;
            _shipOrderHandler = shipOrderHandler;
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
            //var query = new GetOrderByIdQuery(id);
            var order = await _getOrderByIdHandler.HandleAsync(id, ct);

            if (order == null) return NotFound();

            return Ok(order);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders(CancellationToken ct)
        {
            var orders = await _getAllOrdersHandler.HandleAsync(ct);
            return Ok(orders);
        }

        [HttpPut("{id:guid}/ship")]
        public async Task<IActionResult> ShipOrder(Guid id, CancellationToken ct)
        {
            await _shipOrderHandler.HandleAsync(id, ct);
            return NoContent();
        }
    }
}
