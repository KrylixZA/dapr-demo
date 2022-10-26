namespace Application.Helpers;

/// <summary>
/// Provides a contract to help with AES symmetric encryption.
/// </summary>
public interface IAesEncryptionHelper
{
  /// <summary>
  /// Uses symmetric AES encryption to encrypt a string.
  /// </summary>
  /// <param name="stringToEncrypt">The string to encrypt.</param>
  /// <returns>The encrypted representation of the string.</returns>
  Task<string> EncryptStringAsync(string stringToEncrypt);
}