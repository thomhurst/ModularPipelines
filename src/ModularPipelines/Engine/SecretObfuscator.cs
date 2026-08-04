using System.Buffers;
using System.Text;
using Initialization.Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Options;

namespace ModularPipelines.Engine;

/// <summary>
/// Obfuscates sensitive information in strings by replacing secret values with a mask.
/// </summary>
/// <remarks>
/// <para>
/// <b>Thread Safety:</b> This class is thread-safe. The <see cref="Obfuscate"/> method can be
/// called concurrently from multiple threads without external synchronization.
/// </para>
/// <para>
/// <b>Performance:</b> For optimal performance, secrets are sorted by length (longest first)
/// to ensure longer secrets are masked before shorter ones that might be substrings.
/// When case-insensitive matching is enabled, a single-pass algorithm using
/// <see cref="StringComparison.OrdinalIgnoreCase"/> is used. This approach uses .NET's
/// highly optimized string search which performs well for typical log message sizes.
/// For extremely large log outputs with many secrets, consider reducing the number of
/// registered secrets or using case-sensitive matching which uses the more efficient
/// <see cref="System.Text.StringBuilder.Replace(string, string)"/>.
/// </para>
/// </remarks>
/// <threadsafety static="true" instance="true"/>
internal class SecretObfuscator : ISecretObfuscator, IInitializer
{
    private readonly ISecretProvider _secretProvider;
    private readonly IOptions<SecretMaskingOptions> _maskingOptions;
    private readonly object _secretCacheLock = new();

    private SecretCache? _secretCache;

    public int Order => int.MaxValue;

    public SecretObfuscator(
        ISecretProvider secretProvider,
        IOptions<SecretMaskingOptions> maskingOptions)
    {
        _secretProvider = secretProvider;
        _maskingOptions = maskingOptions;
    }

    public Task InitializeAsync()
    {
        // Build system masking is handled by SecretProvider:
        // - Secrets from options are masked during SecretProvider.InitializeAsync()
        // - Secrets added via AddSecret() are masked immediately when added
        // This prevents duplicate masking calls to the build system
        return Task.CompletedTask;
    }

    public string Obfuscate(string? input, object? optionsObject)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }

        var options = _maskingOptions.Value;
        // Ensure mask value is never empty to avoid removing secrets without masking
        var maskValue = string.IsNullOrWhiteSpace(options.MaskValue) ? "**********" : options.MaskValue;
        var caseInsensitive = options.CaseInsensitive;

        var secretCache = GetSecretCache(optionsObject, options, caseInsensitive);
        if (secretCache.SearchValues is null ||
            !input.AsSpan().ContainsAny(secretCache.SearchValues))
        {
            return input;
        }

        // For case-sensitive matching, StringBuilder.Replace is efficient
        // For case-insensitive matching, we need a different approach
        if (caseInsensitive)
        {
            return ObfuscateCaseInsensitive(input, secretCache.Secrets, maskValue);
        }

        return ObfuscateCaseSensitive(input, secretCache.Secrets, maskValue);
    }

    private SecretCache GetSecretCache(
        object? optionsObject,
        SecretMaskingOptions options,
        bool caseInsensitive)
    {
        var registeredSecrets = GetRegisteredSecretCache(caseInsensitive);
        if (optionsObject is null)
        {
            return registeredSecrets;
        }

        var minimumLength = Math.Max(1, options.MinimumSecretLength);
        var extraSecrets = _secretProvider.GetSecretsInObject(optionsObject)
            .Where(secret => secret.Length >= minimumLength)
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        if (extraSecrets.Length == 0)
        {
            return registeredSecrets;
        }

        var missingPatterns = extraSecrets
            .SelectMany(SecretMaskingPatternGenerator.Generate)
            .Where(pattern => !registeredSecrets.ExactSecrets.Contains(pattern))
            .ToArray();
        if (missingPatterns.Length == 0)
        {
            return registeredSecrets;
        }

        return CreateSecretCache(
            registeredSecrets.Secrets.Concat(missingPatterns),
            registeredSecrets.Version,
            caseInsensitive);
    }

    private SecretCache GetRegisteredSecretCache(bool caseInsensitive)
    {
        var version = _secretProvider.Version;
        var currentCache = Volatile.Read(ref _secretCache);
        if (currentCache is not null &&
            (version & 1) == 0 &&
            currentCache.Version == version &&
            currentCache.CaseInsensitive == caseInsensitive &&
            _secretProvider.Version == version)
        {
            return currentCache;
        }

        lock (_secretCacheLock)
        {
            var snapshot = _secretProvider.GetSnapshot();
            currentCache = _secretCache;
            if (currentCache is not null &&
                currentCache.Version == snapshot.Version &&
                currentCache.CaseInsensitive == caseInsensitive)
            {
                return currentCache;
            }

            var newCache = CreateSecretCache(snapshot.Secrets, snapshot.Version, caseInsensitive);
            Volatile.Write(ref _secretCache, newCache);
            return newCache;
        }
    }

    private static SecretCache CreateSecretCache(
        IEnumerable<string> secrets,
        long version,
        bool caseInsensitive)
    {
        var comparer = caseInsensitive ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
        var nonBlankSecrets = secrets
            .Where(secret => !string.IsNullOrWhiteSpace(secret))
            .ToArray();
        var orderedSecrets = nonBlankSecrets
            .Distinct(comparer)
            .OrderByDescending(secret => secret.Length)
            .ToArray();
        var comparison = caseInsensitive ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
        var searchValues = orderedSecrets.Length == 0
            ? null
            : SearchValues.Create(orderedSecrets, comparison);

        return new SecretCache(
            version,
            caseInsensitive,
            orderedSecrets,
            nonBlankSecrets.ToHashSet(StringComparer.Ordinal),
            searchValues);
    }

    /// <summary>
    /// Performs case-sensitive obfuscation using StringBuilder.Replace.
    /// This is the most efficient approach for case-sensitive matching.
    /// </summary>
    private static string ObfuscateCaseSensitive(string input, IReadOnlyList<string> secrets, string maskValue)
    {
        var stringBuilder = new StringBuilder(input);

        foreach (var secret in secrets)
        {
            stringBuilder.Replace(secret, maskValue);
        }

        return stringBuilder.ToString();
    }

    /// <summary>
    /// Performs case-insensitive obfuscation using IndexOf with OrdinalIgnoreCase.
    /// </summary>
    private static string ObfuscateCaseInsensitive(string input, IReadOnlyList<string> secrets, string maskValue)
    {
        var result = input;

        foreach (var secret in secrets)
        {
            result = ReplaceCaseInsensitive(result, secret, maskValue);
        }

        return result;
    }

    /// <summary>
    /// Replaces all occurrences of a pattern in a string, case-insensitively.
    /// </summary>
    private static string ReplaceCaseInsensitive(string input, string pattern, string replacement)
    {
        if (pattern.Length == 0)
        {
            return input;
        }

        var firstIndex = input.IndexOf(pattern, StringComparison.OrdinalIgnoreCase);
        if (firstIndex < 0)
        {
            return input;
        }

        var result = new StringBuilder(input.Length);
        var lastIndex = 0;
        var index = firstIndex;
        while (index >= 0)
        {
            result.Append(input, lastIndex, index - lastIndex);
            result.Append(replacement);
            lastIndex = index + pattern.Length;
            index = input.IndexOf(pattern, lastIndex, StringComparison.OrdinalIgnoreCase);
        }

        // Append the remaining part after the last match
        result.Append(input, lastIndex, input.Length - lastIndex);

        return result.ToString();
    }

    private sealed record SecretCache(
        long Version,
        bool CaseInsensitive,
        string[] Secrets,
        IReadOnlySet<string> ExactSecrets,
        SearchValues<string>? SearchValues);
}
