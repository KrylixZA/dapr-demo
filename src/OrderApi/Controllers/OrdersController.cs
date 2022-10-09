using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Managers;
using OrderApi.Models;

namespace OrderApi.Controllers
{
    /// <summary>
    /// Provides a contract for interacting with orders in the state store.
    /// </summary>
    [Route("orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderManager _orderManager;

        /// <summary>
        /// Instantiates a new instace of the orders controller class.
        /// </summary>
        /// <param name="orderManager">The order manager.</param>
        public OrdersController(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        /// <summary>
        /// Persists a new order to the state storage.
        /// </summary>
        /// <param name="order">The details of the order.</param>
        [HttpPost]
        public async Task<IActionResult> CreateOrder([Required][FromBody]Order order)
        {
            await _orderManager.CreateOrderAsync(order);
            return Ok();
        }

        /// <summary>
        /// Retries all the orders from the state storage.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderManager.GetOrdersAsync();
            return Ok(orders);
        }

        /// <summary>
        /// Retries the details of a specific order based on the order identifier provided..
        /// </summary>
        /// <param name="orderId">The order identifier.</param>
        [HttpGet("/order/{orderId}")]
        public async Task<IActionResult> GetOrderDetails([FromRoute]Guid orderId)
        {
            var orderDetails = await _orderManager.GetOrderDetailsAsync(orderId);
            return Ok(orderDetails);
        }
    }
}

