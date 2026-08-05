using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Options;

/// <summary>
/// Configuration options for secret masking behavior in log output.
/// </summary>
/// <remarks>
/// <para>
/// These options control how secrets are detected and masked in log output.
/// Configure through <see cref="PipelineBuilder.Services"/> and <c>Configure&lt;SecretMaskingOptions&gt;</c>.
/// </para>
/// <para>
/// <strong>Default Behavior:</strong>
/// </para>
/// <list type="bullet">
/// <item>Case-sensitive matching (recommended for most use cases)</item>
/// <item>Minimum secret length of 3 characters to avoid false positives</item>
/// <item>All secrets in a string are replaced with the mask value</item>
/// </list>
/// </remarks>
/// <example>
/// <code>
/// var builder = Pipeline.CreateBuilder();
/// builder.Services.Configure&lt;SecretMaskingOptions&gt;(options =>
/// {
///     options.CaseInsensitive = true;  // Match "Password", "PASSWORD", "password"
///     options.MinimumSecretLength = 4; // Only mask secrets 4+ chars
///     options.MaskValue = "[REDACTED]"; // Custom mask text
/// });
///
/// await builder.ExecutePipelineAsync();
/// </code>
/// </example>
[ExcludeFromCodeCoverage]
public record SecretMaskingOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether secret matching is case-insensitive.
    /// </summary>
    /// <remarks>
    /// When <c>true</c>, a secret value of "MyPassword" will also mask "MYPASSWORD", "mypassword", etc.
    /// Default is <c>false</c> for case-sensitive matching (most secure as it avoids false positives).
    /// </remarks>
    /// <value>
    /// <c>true</c> for case-insensitive matching; <c>false</c> (default) for case-sensitive matching.
    /// </value>
    public bool CaseInsensitive { get; set; }

    /// <summary>
    /// Gets or sets the minimum length a secret must have to be masked.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Secrets shorter than this length are excluded from framework substring masking to prevent excessive false positives.
    /// For example, a 1-character secret like "a" would mask all "a" characters in output.
    /// A warning is logged, and native build-system masking is still requested when supported.
    /// </para>
    /// <para>
    /// The default value of 1 ensures all non-empty secrets are masked, maintaining backward
    /// compatibility. Consider increasing to 3+ for production to avoid masking common short strings.
    /// </para>
    /// </remarks>
    /// <value>
    /// Minimum secret length. Default is 1. Set to 0 to disable minimum length check (same effect as 1).
    /// </value>
    public int MinimumSecretLength { get; set; } = 1;

    /// <summary>
    /// Gets or sets the string used to replace secret values in output.
    /// </summary>
    /// <remarks>
    /// If set to an empty or whitespace string, the default mask value ("**********") will be used
    /// to prevent secrets from being removed without visible masking.
    /// </remarks>
    /// <value>
    /// The mask string. Default is "**********".
    /// </value>
    public string MaskValue { get; set; } = "**********";

    /// <summary>
    /// Gets or sets the configuration section paths whose leaf values are registered as secrets.
    /// </summary>
    /// <remarks>
    /// Paths use the standard configuration delimiter, for example <c>Secrets</c> or
    /// <c>Tenants:Production:Credentials</c>. Values are collected once during pipeline startup.
    /// Missing sections and empty values are ignored.
    /// </remarks>
    public IReadOnlyList<string> MaskedConfigurationSections { get; set; } = [];

    /// <summary>
    /// Gets default secret masking options with case-sensitive matching and minimum length of 3.
    /// </summary>
    public static SecretMaskingOptions Default { get; } = new();
}
