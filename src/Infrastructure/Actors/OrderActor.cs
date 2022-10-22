using System;
using System.Xml.Linq;
using Application.Actors;
using Application.Repositories;
using Dapr.Actors.Runtime;
using Domain;
using Domain.Constants;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Models;

namespace Infrastructure.Actors;

/// <summary>
/// Implements a contract for my test actor.
/// </summary>
public class OrderActor : Actor, IOrderActor, IRemindable
{
  private readonly IOrderStateRepository _orderStateRepository;
  private readonly IOrderPubSubRepository _orderPubSubRepository;

  /// <summary>
  /// The actor type.
  /// </summary>
  public static string ActorType => nameof(OrderActor);

  // The constructor must accept ActorHost as a parameter, and can also accept additional
  // parameters that will be retrieved from the dependency injection container
  //
  /// <summary>
  /// Initializes a new instance of MyActor
  /// </summary>
  /// <param name="host">The Dapr.Actors.Runtime.ActorHost that will host this actor instance.</param>
  /// <param name="orderStateRepository">The order state repository.</param>
  /// <param name="orderPubSubRepository">The order pub/sub repository.</param>
  public OrderActor(
    ActorHost host,
    IOrderStateRepository orderStateRepository,
    IOrderPubSubRepository orderPubSubRepository)
      : base(host)
  {
    _orderStateRepository = orderStateRepository;
    _orderPubSubRepository = orderPubSubRepository;
  }

  /// <inheritdoc />
  public async Task CreateOrderAsync(Order order)
  {
    await Task.WhenAll(
      _orderStateRepository.CreateOrderAsync(order),
      StateManager.SetStateAsync<Order>(
        DaprComponents.OrderActorStateStore,  // state name
        order)                                // actor state
      );
  }

  /// <inheritdoc />
  public async Task CheckoutOrderAsync(Guid orderId)
  {
    var order = await _orderStateRepository.GetOrderAsync(orderId);
    if (order is null)
    {
      throw new OrderNotFoundException();
    }

    order.OrderUpdatedDateTimeUtc = DateTime.UtcNow;
    order.OrderState = OrderState.CheckOut;

    await Task.WhenAll(
      _orderStateRepository.CheckoutOrderAsync(order),
      _orderPubSubRepository.PublishOrderForCheckoutAsync(order),
      StateManager.SetStateAsync<Order>(
        DaprComponents.OrderActorStateStore,  // state name
        order)                                // actor state
      );
  }

  #region demo code
  /// <summary>
  /// This method is called whenever an actor is activated.
  /// An actor is activated the first time any of its methods are invoked.
  /// </summary>
  protected override Task OnActivateAsync()
  {
    // Provides opportunity to perform some optional setup.
    Console.WriteLine($"Activating actor id: {this.Id}");
    return Task.CompletedTask;
  }

  /// <summary>
  /// This method is called whenever an actor is deactivated after a period of inactivity.
  /// </summary>
  protected override Task OnDeactivateAsync()
  {
    // Provides Opporunity to perform optional cleanup.
    Console.WriteLine($"Deactivating actor id: {this.Id}");
    return Task.CompletedTask;
  }

  /// <summary>
  /// Register MyReminder reminder with the actor
  /// </summary>
  public async Task RegisterReminder()
  {
    await this.RegisterReminderAsync(
        "MyReminder",              // The name of the reminder
        null,                      // User state passed to IRemindable.ReceiveReminderAsync()
        TimeSpan.FromSeconds(5),   // Time to delay before invoking the reminder for the first time
        TimeSpan.FromSeconds(5));  // Time interval between reminder invocations after the first invocation
  }

  /// <summary>
  /// Unregister MyReminder reminder with the actor
  /// </summary>
  public Task UnregisterReminder()
  {
    Console.WriteLine("Unregistering MyReminder...");
    return this.UnregisterReminderAsync("MyReminder");
  }

  /// <summary>
  /// Implement IRemindeable.ReceiveReminderAsync() which is call back invoked when an actor reminder is triggered.
  /// </summary>
  public Task ReceiveReminderAsync(string reminderName, byte[] state, TimeSpan dueTime, TimeSpan period)
  {
    Console.WriteLine("ReceiveReminderAsync is called!");
    return Task.CompletedTask;
  }

  /// <summary>
  /// Register MyTimer timer with the actor
  /// </summary>
  public Task RegisterTimer()
  {
    return this.RegisterTimerAsync(
        "MyTimer",                  // The name of the timer
        nameof(this.OnTimerCallBack),       // Timer callback
        null,                       // User state passed to OnTimerCallback()
        TimeSpan.FromSeconds(5),    // Time to delay before the async callback is first invoked
        TimeSpan.FromSeconds(5));   // Time interval between invocations of the async callback
  }

  /// <summary>
  /// Unregister MyTimer timer with the actor
  /// </summary>
  public Task UnregisterTimer()
  {
    Console.WriteLine("Unregistering MyTimer...");
    return this.UnregisterTimerAsync("MyTimer");
  }

  /// <summary>
  /// Timer callback once timer is expired
  /// </summary>
  private Task OnTimerCallBack(byte[] data)
  {
    Console.WriteLine("OnTimerCallBack is called!");
    return Task.CompletedTask;
  }
  #endregion
}