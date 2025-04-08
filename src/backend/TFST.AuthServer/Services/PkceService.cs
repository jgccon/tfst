using Microsoft.Extensions.Caching.Distributed;
using System.Text;

namespace TFST.AuthServer.Services;

public class PkceService(IDistributedCache cache)
{
    private readonly IDistributedCache _cache = cache;

    public async Task<(string codeChallenge, string state)> CreatePkceAsync()
    {
        var codeVerifier = GenerateSecureRandomString();
        var state = GenerateSecureRandomString();

        await _cache.SetStringAsync(
            $"pkce_{state}",
            codeVerifier,
            new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });

        return (GenerateCodeChallenge(codeVerifier), state);
    }

    public async Task<string?> GetCodeVerifierAsync(string state)
    {
        var codeVerifier = await _cache.GetStringAsync($"pkce_{state}");
        if (codeVerifier != null)
        {
            // Remove after use
            await _cache.RemoveAsync($"pkce_{state}");
        }
        return codeVerifier;
    }

    private string GenerateSecureRandomString()
    {
        var randomBytes = new byte[32];
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Base64UrlEncode(randomBytes);
    }

    private string GenerateCodeChallenge(string codeVerifier)
    {
        using var sha256 = System.Security.Cryptography.SHA256.Create();
        var challengeBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(codeVerifier));
        return Base64UrlEncode(challengeBytes);
    }

    private string Base64UrlEncode(byte[] input)
    {
        var output = Convert.ToBase64String(input);
        output = output.Split('=')[0]; // Remove any trailing '='s
        output = output.Replace('+', '-'); // Replace '+' with '-'
        output = output.Replace('/', '_'); // Replace '/' with '_'
        return output;
    }
}