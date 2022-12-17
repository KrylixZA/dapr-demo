using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text.Json;
using Application.Helpers;
using Dapr.Client;
using Domain.Constants;
using Domain.Exceptions;

namespace Infrastructure.Helpers;

/// <summary>
/// Implements a contract to help with AES symmetric encryption.
/// </summary>
public class AesEncryptionHelper : IAesEncryptionHelper
{
  private readonly DaprClient _daprClient;

  private const string AesEncryptionKey = "aesEncryptionKey";
  private const string AesEncryptionVector = "aesEncryptionVector";

  /// <summary>
  /// Instantiates a new instance of the AesEncryptionHelper class.
  /// </summary>
  public AesEncryptionHelper()
  {
    _daprClient = new DaprClientBuilder().Build();
  }

  /// <inheritdoc />
  public async Task<string> EncryptObjectAsync<T>(T objectToEncrypt) where T : class
  {
    var key = (await _daprClient.GetSecretAsync(DaprComponents.AesSecretStore, AesEncryptionKey))[AesEncryptionKey];
    var vector = (await _daprClient.GetSecretAsync(DaprComponents.AesSecretStore, AesEncryptionVector))[AesEncryptionVector];

    using var aes = Aes.Create();
    aes.Key = Convert.FromBase64String(key);
    aes.IV = Convert.FromBase64String(vector);
    var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
    await using var msEncrypt = new MemoryStream();
    await using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
    await using (var swEncrypt = new StreamWriter(csEncrypt))
    {
      await swEncrypt.WriteAsync(JsonSerializer.Serialize(objectToEncrypt));
    }
    var encrypted = msEncrypt.ToArray();
    return Convert.ToBase64String(encrypted);
  }

  /// <inheritdoc />
  public async Task<T> DecryptToObjectAsync<T>(string stringToDecrypt) where T : class
  {
    var key = (await _daprClient.GetSecretAsync(DaprComponents.AesSecretStore, AesEncryptionKey))[AesEncryptionKey];
    var vector = (await _daprClient.GetSecretAsync(DaprComponents.AesSecretStore, AesEncryptionVector))[AesEncryptionVector];

    using var aes = Aes.Create();
    aes.Key = Convert.FromBase64String(key);
    aes.IV = Convert.FromBase64String(vector);
    var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
    await using var msDecrypt = new MemoryStream(Convert.FromBase64String(stringToDecrypt));
    await using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
    using (var swDecrypt = new StreamReader(csDecrypt))
    {
      var jsonObject = await swDecrypt.ReadToEndAsync();
      var objectToReturn = JsonSerializer.Deserialize<T>(jsonObject);
      if (objectToReturn is null)
      {
        throw new EncryptionException();
      }
      return objectToReturn;
    }
  }
}