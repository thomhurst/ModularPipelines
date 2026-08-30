using Microsoft.Extensions.Options;
using ModularPipelines.Constants;
using ModularPipelines.Engine;
using ModularPipelines.Options;
using ModularPipelines.Secrets;

namespace ModularPipelines.Http;

internal static class HttpBodySecretRedactor
{
    public static string Redact(
        string body,
        bool isTruncated,
        ISecretObfuscator secretObfuscator,
        ISecretProvider secretProvider,
        IOptions<SecretMaskingOptions> maskingOptions)
    {
        var boundarySafeBody = isTruncated
            ? RedactPartialSecretAtBoundary(body, secretProvider.GetSnapshot().Secrets, maskingOptions.Value)
            : body;
        return secretObfuscator.Obfuscate(boundarySafeBody, null);
    }

    private static string RedactPartialSecretAtBoundary(
        string body,
        IReadOnlyList<string> secrets,
        SecretMaskingOptions options)
    {
        if (body.Length == 0)
        {
            return body;
        }

        var comparison = options.CaseInsensitive
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;
        var longestPartialMatch = 0;

        foreach (var secret in secrets)
        {
            var maximumLength = Math.Min(body.Length, secret.Length - 1);
            for (var length = maximumLength; length > longestPartialMatch; length--)
            {
                if (body.AsSpan(body.Length - length, length)
                    .Equals(secret.AsSpan(0, length), comparison))
                {
                    longestPartialMatch = length;
                    break;
                }
            }
        }

        if (longestPartialMatch == 0)
        {
            return body;
        }

        var mask = string.IsNullOrWhiteSpace(options.MaskValue)
            ? LoggingConstants.SecretMask
            : options.MaskValue;
        return string.Concat(body.AsSpan(0, body.Length - longestPartialMatch), mask);
    }
}
