﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using Application.Managers;

namespace OrderApi.Controllers;

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
  /// <remarks>
  /// As this is all part of a demo, we will be spinning up a virtual actor to represent the order.
  /// </remarks>
  /// <param name="order">The details of the order.</param>
  [HttpPost]
  public async Task<IActionResult> CreateOrder([Required][FromBody] Order order)
  {
    await _orderManager.CreateOrderAsync(order);
    return Ok();
  }

  /// <summary>
  /// Begins the checkout process for an order.
  /// </summary>
  /// <remarks>
  /// Because an order is represented by a virtual actor, this will initiate the following changes:
  /// 
  /// </remarks>
  /// <param name="orderId">The order unique identifier.</param>
  [HttpPost("/checkout/order/{orderId}")]
  public async Task<IActionResult> CheckoutOrder([Required][FromRoute] Guid orderId)
  {
    await _orderManager.CheckoutOrderAsync(orderId);
    return Ok();
  }
}