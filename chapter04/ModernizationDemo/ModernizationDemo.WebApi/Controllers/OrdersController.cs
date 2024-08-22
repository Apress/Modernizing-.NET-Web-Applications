using ModernizationDemo.BusinessLogic.Models;
using ModernizationDemo.BusinessLogic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;

namespace ModernizationDemo.WebApi.Controllers
{
    [RoutePrefix("api/Orders")]
    public class OrdersController : ApiController
    {
        private readonly OrderService service = new OrderService();

        [Route]
        public List<OrderModel> GetOrders()
        {
            return service.GetOrders();
        }

        [Route("{id:int}")]
        [ResponseType(typeof(OrderModel))]
        public IHttpActionResult GetOrder(int id)
        {
            return Ok(service.GetOrder(id));
        }

        [HttpPost]
        [Route]
        public int AddOrder([FromBody] OrderCreateModel order)
        {
            return service.AddOrder(order);
        }

        [HttpPost]
        [Route("calculate")]
        public decimal CalculateTotalPrice([FromBody] OrderCreateModel order)
        {
            return service.CalculateTotalPrice(order);
        }

        [HttpPost]
        [Route("{id:int}/complete")]
        public void CompleteOrder(int id)
        {
            service.CompleteOrder(id);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public void CancelOrder(int id)
        {
            service.CancelOrder(id);
        }

    }
}
