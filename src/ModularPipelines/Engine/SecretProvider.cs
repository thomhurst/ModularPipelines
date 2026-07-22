using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Initialization.Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Engine;

/// <summary>
/// Provides secret discovery from IOptions objects and programmatic secret registration.
/// </summary>
/// <remarks>
/// <para>
/// <b>Thread Safety:</b> This class is thread-safe. The <see cref="AddSecret"/> and
/// <see cref="AddSecrets(IEnumerable{string})"/> methods can be called concurrently from multiple threads.
/// </para>
/// <para>
/// <b>Secret Sources:</b>
/// </para>
/// <list type="bullet">
/// <item>Properties marked with <see cref="SecretValueAttribute"/> on IOptions classes (discovered at initialization)</item>
/// <item>Secrets registered programmatically via <see cref="ISecretRegistry"/> (can be added at any time)</item>
/// </list>
/// </remarks>
/// <threadsafety static="true" instance="true"/>
internal class SecretProvider : ISecretProvider, ISecretRegistry, IInitializer
{
    private static readonly ConcurrentDictionary<Type, IReadOnlyList<SecretPropertyAccessor>> ReflectionAccessorsCache = new();

    private readonly IOptionsProvider _optionsProvider;
    private readonly IBuildSystemSecretMasker _buildSystemSecretMasker;
    private readonly IOptions<SecretMaskingOptions> _maskingOptions;
    private readonly ILogger<SecretProvider> _logger;
    private readonly ConcurrentDictionary<string, byte> _secrets = new();
    private readonly ConcurrentDictionary<string, byte> _nativeMaskPatterns = new();
    private readonly ConcurrentDictionary<string, byte> _shortSecretWarnings = new();
    private readonly object _initLock = new();

    private volatile bool _initialized;

    /// <summary>
    /// Gets all registered secrets.
    /// </summary>
    /// <remarks>
    /// <b>Thread Safety:</b> Enumeration returns a point-in-time snapshot of secrets.
    /// Secrets added during enumeration will not be included in the current iteration.
    /// Each enumeration creates a new snapshot.
    /// </remarks>
    public IEnumerable<string> Secrets => _secrets.Keys;

    public SecretProvider(
        IOptionsProvider optionsProvider,
        IBuildSystemSecretMasker buildSystemSecretMasker,
        IOptions<SecretMaskingOptions> maskingOptions,
        ILogger<SecretProvider> logger)
    {
        _optionsProvider = optionsProvider;
        _buildSystemSecretMasker = buildSystemSecretMasker;
        _maskingOptions = maskingOptions;
        _logger = logger;
    }

    /// <inheritdoc />
    public void AddSecret(string? secret)
    {
        if (string.IsNullOrWhiteSpace(secret))
        {
            return;
        }

        var patterns = SecretMaskingPatternGenerator.Generate(secret);
        RegisterNativeMaskPatterns(patterns);

        var minimumLength = Math.Max(1, _maskingOptions.Value.MinimumSecretLength);
        if (secret.Length < minimumLength)
        {
            if (_shortSecretWarnings.TryAdd(secret, 0))
            {
                _logger.LogWarning(
                    "A secret with length {SecretLength} is shorter than MinimumSecretLength {MinimumSecretLength}. " +
                    "Framework log masking is disabled for this value; native build-system masking was requested.",
                    secret.Length,
                    minimumLength);
            }

            return;
        }

        foreach (var pattern in patterns)
        {
            _secrets.TryAdd(pattern, 0);
        }
    }

    /// <inheritdoc />
    public void AddSecrets(IEnumerable<string?> secrets)
    {
        foreach (var secret in secrets)
        {
            AddSecret(secret);
        }
    }

    /// <inheritdoc />
    public void AddSecrets(params string?[] secrets)
    {
        AddSecrets((IEnumerable<string?>) secrets);
    }

    public IEnumerable<string> GetSecretsInObject(object? value)
    {
        if (value is null)
        {
            yield break;
        }

        var type = value.GetType();
        if (!GeneratedSecretMetadata.TryGetAccessors(type, out var secretProperties))
        {
            secretProperties = ReflectionAccessorsCache.GetOrAdd(type, GetSecretProperties);
        }

        foreach (var property in secretProperties)
        {
            foreach (var secret in GetSecretsFromProperty(property, value))
            {
                yield return secret;
            }
        }
    }

    private static IEnumerable<string> GetSecretsFromProperty(
        SecretPropertyAccessor property,
        object value)
    {
        var propertyValue = property.Getter(value);
        var secretValueKeys = property.SecretValueKeys ?? Array.Empty<string>();

        if (secretValueKeys.Count == 0)
        {
            var secret = propertyValue?.ToString();
            if (!string.IsNullOrWhiteSpace(secret))
            {
                yield return secret;
            }

            yield break;
        }

        if (propertyValue is not IEnumerable<KeyValue> keyValues)
        {
            yield break;
        }

        foreach (var keyValue in keyValues)
        {
            if (secretValueKeys.Contains(keyValue.Key, StringComparer.OrdinalIgnoreCase) &&
                !string.IsNullOrWhiteSpace(keyValue.Value))
            {
                yield return keyValue.Value;
            }
        }
    }

    public Task InitializeAsync()
    {
        // Use double-checked locking pattern for thread-safety
        // The volatile read of _initialized before the lock provides a fast-path
        // for subsequent calls after initialization is complete
        if (_initialized)
        {
            return Task.CompletedTask;
        }

        lock (_initLock)
        {
            // Re-check inside lock to prevent race condition
            if (_initialized)
            {
                return Task.CompletedTask;
            }

            AddSecrets(GetSecrets(_optionsProvider.GetOptions()));

            _initialized = true;
        }

        return Task.CompletedTask;
    }

    [RequiresUnreferencedCode("Reflection fallback requires SecretValue-attributed properties. Ensure ModularPipelines.SourceGenerator runs for trim-safe secret access.")]
    private static IReadOnlyList<SecretPropertyAccessor> GetSecretProperties(Type type)
    {
        return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Concat(type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance))
            .Where(m => m.GetCustomAttribute<SecretValueAttribute>() is not null)
            .Select(property => new SecretPropertyAccessor(
                property.Name,
                property.GetValue,
                property.GetCustomAttribute<SecretValueAttribute>()!.Keys))
            .ToArray();
    }

    private IEnumerable<string> GetSecrets(IEnumerable<object?> options)
    {
        foreach (var option in options)
        {
            foreach (var secret in GetSecretsInObject(option))
            {
                yield return secret;
            }
        }
    }

    private void RegisterNativeMaskPatterns(IEnumerable<string> patterns)
    {
        var newPatterns = patterns.Where(pattern => _nativeMaskPatterns.TryAdd(pattern, 0)).ToArray();
        if (newPatterns.Length > 0)
        {
            _buildSystemSecretMasker.MaskSecrets(newPatterns);
        }
    }
}
