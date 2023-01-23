namespace Domain.Constants;

/// <summary>
/// A set of constants pointing to the dapr components.
/// </summary>
public static class DaprComponents
{
  /// <summary>
  /// The order state store component.
  /// </summary>
  public const string StateStore = "statestore";

  /// <summary>
  /// The pubsub component. 
  /// </summary>
  public const string PubSub = "pubsub";

  /// <summary>
  /// The component holding the AES secrets.
  /// </summary>
  public const string Secrets = "secrets";
}