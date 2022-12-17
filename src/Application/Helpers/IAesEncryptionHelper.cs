namespace Application.Helpers;

/// <summary>
/// Provides a contract to help with AES symmetric encryption.
/// </summary>
public interface IAesEncryptionHelper
{
  /// <summary>
  /// Uses symmetric AES encryption to encrypt an object.
  /// </summary>
  /// <param name="objectToEncrypt">The object to encrypt. This will be converted to a JSON object before encryption.</param>
  /// <typeparam name="T">The type of class to encrypt.</typeparam>
  /// <returns>The encrypted representation of the string.</returns>
  Task<string> EncryptObjectAsync<T>(T objectToEncrypt) where T : class;

  /// <summary>
  /// Uses symmetric AES encryption to decrypt a string to an object.
  /// </summary>
  /// <param name="stringToDecrypt">The string to decrypt.</param>
  /// <typeparam name="T">The type of class to return after decryption.</typeparam>
  /// <returns>If possible, the class representation of the encrypted string.</returns>
  Task<T> DecryptToObjectAsync<T>(string stringToDecrypt) where T : class;
}