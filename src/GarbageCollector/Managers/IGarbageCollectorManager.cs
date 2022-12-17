namespace GarbageCollector.Managers;

/// <summary>
/// Provides a contract for managing order events and garbage collecting any completed orders.
/// </summary>
public interface IGarbageCollectorManager
{
  /// <summary>
  /// Processes the cloud event published as an encrypted string.
  /// The encrypted string will first be decrypted to an Order model.
  /// If the order is in a "Complete" state, we will process the event.
  /// Any other events will be ignored.
  /// </summary>
  /// <param name="encryptedOrderEvent">The order event as an encrypted payload.</param>
  Task ProcessOrderEventAsync(string encryptedOrderEvent);
}