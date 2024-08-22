using Microsoft.AspNetCore.Mvc;
using ModernizationDemo.BusinessLogic.Models;
using ModernizationDemo.BusinessLogic.Services;

namespace ModernizationDemo.WebApiCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService service = new OrderService();

        [HttpGet]
        public List<OrderModel> GetOrders()
        {
            return service.GetOrders();
        }

        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType<OrderModel>(200)]
        public IActionResult GetOrder(int id)
        {
            return Ok(service.GetOrder(id));
        }

        [HttpPost]
        public int AddOrder(OrderCreateModel order)
        {
            return service.AddOrder(order);
        }

        [HttpPost]
        [Route("calculate")]
        public decimal CalculateTotalPrice(OrderCreateModel order)
        {
            return service.CalculateTotalPrice(order);
        }

        [HttpPost]
        [Route("{id:int}/complete")]
        [ProducesResponseType(204)]
        public void CompleteOrder(int id)
        {
            service.CompleteOrder(id);
        }

        [HttpDelete]
        [Route("{id:int}")]
        [ProducesResponseType(204)]
        public void CancelOrder(int id)
        {
            service.CancelOrder(id);
        }

    }
}
