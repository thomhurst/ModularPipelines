namespace ModularPipelines.Engine;

/// <summary>
/// Provides functionality to register secrets programmatically at runtime.
/// Secrets registered here will be automatically masked in log output.
/// </summary>
/// <remarks>
/// <para>
/// Use this interface when secrets are obtained dynamically (e.g., from a vault or API)
/// and cannot be marked with <see cref="Attributes.SecretValueAttribute"/>.
/// </para>
/// <para>
/// Secrets are automatically registered with the CI/CD build system's secret masking
/// feature (GitHub Actions, Azure Pipelines, etc.) when running in a supported environment.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class MyModule : Module&lt;bool&gt;
/// {
///     private readonly ISecretRegistry _secrets;
///
///     public MyModule(ISecretRegistry secrets)
///     {
///         _secrets = secrets;
///     }
///
///     protected override async Task&lt;bool&gt; ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
///     {
///         // Get secret from external source
///         var apiKey = await GetApiKeyFromVaultAsync();
///
///         // Register it for masking
///         _secrets.AddSecret(apiKey);
///
///         // Now use it safely - it will be masked in logs
///         context.Logger.LogInformation("Using API key: {ApiKey}", apiKey);
///
///         return true;
///     }
/// }
/// </code>
/// </example>
public interface ISecretRegistry
{
    /// <summary>
    /// Registers a secret value for masking in log output.
    /// </summary>
    /// <param name="secret">The secret value to mask. Null or whitespace values are ignored.</param>
    /// <remarks>
    /// <para>
    /// The secret will be masked in all subsequent log output.
    /// If running in a supported CI/CD environment, the secret will also be registered
    /// with the build system's native masking feature.
    /// </para>
    /// <para>
    /// This method is thread-safe and can be called from multiple modules concurrently.
    /// </para>
    /// </remarks>
    void AddSecret(string? secret);

    /// <summary>
    /// Registers multiple secret values for masking in log output.
    /// </summary>
    /// <param name="secrets">The secret values to mask. Null or whitespace values are ignored.</param>
    /// <remarks>
    /// <para>
    /// This is a convenience method for registering multiple secrets at once.
    /// Each secret will be processed according to the same rules as <see cref="AddSecret"/>.
    /// </para>
    /// <para>
    /// This method is thread-safe and can be called from multiple modules concurrently.
    /// </para>
    /// </remarks>
    void AddSecrets(IEnumerable<string?> secrets);

    /// <summary>
    /// Registers multiple secret values for masking in log output.
    /// </summary>
    /// <param name="secrets">The secret values to mask. Null or whitespace values are ignored.</param>
    /// <remarks>
    /// <para>
    /// This is a convenience method for registering multiple secrets at once using params.
    /// Each secret will be processed according to the same rules as <see cref="AddSecret"/>.
    /// </para>
    /// <para>
    /// This method is thread-safe and can be called from multiple modules concurrently.
    /// </para>
    /// </remarks>
    void AddSecrets(params string?[] secrets);
}
