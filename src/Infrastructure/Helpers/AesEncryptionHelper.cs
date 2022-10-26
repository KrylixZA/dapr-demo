using System.Net.Sockets;
using System.Security.Cryptography;
using Application.Helpers;
using Dapr.Client;

namespace Infrastructure.Helpers;

/// <summary>
/// Implements a contract to help with AES symmetric encryption.
/// </summary>
public class AesEncryptionHelper : IAesEncryptionHelper
{
  private readonly DaprClient _daprClient;

  /// <summary>
  /// Instantiates a new instance of the AesEncryptionHelper class.
  /// </summary>
  public AesEncryptionHelper()
  {
    _daprClient = new DaprClientBuilder().Build();
  }

  /// <inheritdoc />
  public async Task<string> EncryptStringAsync(string stringToEncrypt)
  {
    var key = (await _daprClient.GetSecretAsync("aes", "aesEncryptionKey"))["aesEncryptionKey"];
    var vector = (await _daprClient.GetSecretAsync("aes", "aesEncryptionVector"))["aesEncryptionVector"];

    using var aes = Aes.Create();
    aes.Key = Convert.FromBase64String(key);
    aes.IV = Convert.FromBase64String(vector);
    var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
    await using var msEncrypt = new MemoryStream();
    await using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
    await using (var swEncrypt = new StreamWriter(csEncrypt))
    {
      await swEncrypt.WriteAsync(stringToEncrypt);
    }
    var encrypted = msEncrypt.ToArray();
    return Convert.ToBase64String(encrypted);
  }
}