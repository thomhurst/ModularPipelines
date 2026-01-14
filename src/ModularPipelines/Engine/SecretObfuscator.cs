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
/// <see cref="StringComparison.OrdinalIgnoreCase"/> is used.
/// </para>
/// </remarks>
/// <threadsafety static="true" instance="true"/>
internal class SecretObfuscator : ISecretObfuscator, IInitializer
{
    private readonly IBuildSystemSecretMasker _buildSystemSecretMasker;
    private readonly ISecretProvider _secretProvider;
    private readonly IOptions<SecretMaskingOptions> _maskingOptions;

    public int Order => int.MaxValue;

    public SecretObfuscator(
        IBuildSystemSecretMasker buildSystemSecretMasker,
        ISecretProvider secretProvider,
        IOptions<SecretMaskingOptions> maskingOptions)
    {
        _buildSystemSecretMasker = buildSystemSecretMasker;
        _secretProvider = secretProvider;
        _maskingOptions = maskingOptions;
    }

    public Task InitializeAsync()
    {
        _buildSystemSecretMasker.MaskSecrets(_secretProvider.Secrets);
        return Task.CompletedTask;
    }

    public string Obfuscate(string? input, object? optionsObject)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }

        var options = _maskingOptions.Value;
        var maskValue = options.MaskValue;
        var caseInsensitive = options.CaseInsensitive;

        var secretsFromExtraObject = _secretProvider.GetSecretsInObject(optionsObject);

        // Combine all secrets and sort by length (longest first) to ensure
        // longer secrets are replaced before shorter ones that might be substrings
        var allSecrets = _secretProvider.Secrets
            .Concat(secretsFromExtraObject)
            .Where(s => !string.IsNullOrWhiteSpace(s) && s.Length >= options.MinimumSecretLength)
            .Distinct(caseInsensitive ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal)
            .OrderByDescending(s => s.Length)
            .ToList();

        if (allSecrets.Count == 0)
        {
            return input;
        }

        // For case-sensitive matching, StringBuilder.Replace is efficient
        // For case-insensitive matching, we need a different approach
        if (caseInsensitive)
        {
            return ObfuscateCaseInsensitive(input, allSecrets, maskValue);
        }

        return ObfuscateCaseSensitive(input, allSecrets, maskValue);
    }

    /// <summary>
    /// Performs case-sensitive obfuscation using StringBuilder.Replace.
    /// This is the most efficient approach for case-sensitive matching.
    /// </summary>
    private static string ObfuscateCaseSensitive(string input, List<string> secrets, string maskValue)
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
    private static string ObfuscateCaseInsensitive(string input, List<string> secrets, string maskValue)
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

        var result = new StringBuilder(input.Length);
        var lastIndex = 0;
        int index;

        while ((index = input.IndexOf(pattern, lastIndex, StringComparison.OrdinalIgnoreCase)) >= 0)
        {
            result.Append(input, lastIndex, index - lastIndex);
            result.Append(replacement);
            lastIndex = index + pattern.Length;
        }

        // Append the remaining part after the last match
        result.Append(input, lastIndex, input.Length - lastIndex);

        return result.ToString();
    }
}
