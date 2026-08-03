using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Initialization.Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Exceptions;
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
/// <item>Leaf values beneath configured <see cref="SecretMaskingOptions.MaskedConfigurationSections"/> paths</item>
/// <item>Secrets registered programmatically via <see cref="ISecretRegistry"/> (can be added at any time)</item>
/// </list>
/// </remarks>
/// <threadsafety static="true" instance="true"/>
internal class SecretProvider : ISecretProvider, ISecretRegistry, IInitializer
{
    private static readonly ConcurrentDictionary<Assembly, bool> SecretAttributeReferenceCache = new();
    private static readonly ConcurrentDictionary<Type, IReadOnlyList<SecretPropertyAccessor>> ReflectionAccessorsCache = new();

    private readonly IOptionsProvider _optionsProvider;
    private readonly IBuildSystemSecretMasker _buildSystemSecretMasker;
    private readonly IOptions<SecretMaskingOptions> _maskingOptions;
    private readonly ILogger<SecretProvider> _logger;
    private readonly IConfiguration? _configuration;
    private readonly HashSet<string> _secrets = new(StringComparer.Ordinal);
    private readonly ConcurrentDictionary<string, byte> _nativeMaskPatterns = new();
    private readonly ConcurrentDictionary<string, byte> _shortSecretWarnings = new();
    private readonly object _initLock = new();
    private readonly object _secretsLock = new();

    private long _version;
    private volatile bool _initialized;

    /// <inheritdoc />
    public long Version => Volatile.Read(ref _version);

    /// <summary>
    /// Gets all registered secrets.
    /// </summary>
    /// <remarks>
    /// <b>Thread Safety:</b> Enumeration returns a point-in-time snapshot of secrets.
    /// Secrets added during enumeration will not be included in the current iteration.
    /// Each enumeration creates a new snapshot.
    /// </remarks>
    public IEnumerable<string> Secrets => GetSnapshot().Secrets;

    public SecretProvider(
        IOptionsProvider optionsProvider,
        IBuildSystemSecretMasker buildSystemSecretMasker,
        IOptions<SecretMaskingOptions> maskingOptions,
        ILogger<SecretProvider> logger,
        IConfiguration? configuration = null)
    {
        _optionsProvider = optionsProvider;
        _buildSystemSecretMasker = buildSystemSecretMasker;
        _maskingOptions = maskingOptions;
        _logger = logger;
        _configuration = configuration;
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

        lock (_secretsLock)
        {
            if (patterns.All(_secrets.Contains))
            {
                return;
            }

            // Odd versions mark an in-progress publication so readers cannot reuse
            // a cache while the matching secret collection is being updated.
            Interlocked.Increment(ref _version);
            foreach (var pattern in patterns)
            {
                _secrets.Add(pattern);
            }

            Interlocked.Increment(ref _version);
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

    /// <inheritdoc />
    public SecretSnapshot GetSnapshot()
    {
        lock (_secretsLock)
        {
            return new SecretSnapshot(Version, _secrets.ToArray());
        }
    }

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2026",
        Justification = "Processed C# assemblies require exact generated metadata. Unprocessed assemblies use a reflection fallback and are not trim-safe.")]
    public IEnumerable<string> GetSecretsInObject(object? value)
    {
        if (value is null)
        {
            yield break;
        }

        var type = value.GetType();
        if (!GeneratedSecretMetadata.TryGetAccessors(type, out var secretProperties))
        {
            if (GeneratedSecretMetadata.IsAssemblyProcessed(type.Assembly))
            {
                throw new MissingSecretMetadataException(type);
            }
            else if (CanReferenceSecretValueAttribute(type.Assembly))
            {
                secretProperties = ReflectionAccessorsCache.GetOrAdd(type, GetSecretProperties);
            }
            else
            {
                yield break;
            }
        }

        foreach (var property in secretProperties)
        {
            foreach (var secret in GetSecretsFromProperty(property, value))
            {
                yield return secret;
            }
        }
    }

    /// <inheritdoc />
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
            AddSecrets(GetConfiguredSectionSecrets());

            _initialized = true;
        }

        return Task.CompletedTask;
    }

    private static IEnumerable<string> GetSecretsFromProperty(
        SecretPropertyAccessor property,
        object value)
    {
        var propertyValue = property.Getter(value);
        var secretValueKeys = property.SecretValueKeys ?? Array.Empty<string>();

        if (secretValueKeys.Count == 0)
        {
            foreach (var secret in NormalizeSecrets(propertyValue))
            {
                if (!string.IsNullOrWhiteSpace(secret))
                {
                    yield return secret;
                }
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

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2026",
        Justification = "Generated assemblies register themselves. Reference inspection only excludes third-party assemblies that cannot declare SecretValue properties.")]
    private static bool CanReferenceSecretValueAttribute(Assembly assembly)
    {
        return SecretAttributeReferenceCache.GetOrAdd(assembly, static candidate =>
        {
            var attributeAssembly = typeof(SecretValueAttribute).Assembly;
            var attributeAssemblyName = attributeAssembly.GetName().Name;
            return candidate == attributeAssembly
                   || candidate.GetReferencedAssemblies().Any(
                       reference => string.Equals(
                           reference.Name,
                           attributeAssemblyName,
                           StringComparison.Ordinal));
        });
    }

    [RequiresUnreferencedCode("Assemblies without generated secret metadata require reflection and are not trim-safe.")]
    private static IReadOnlyList<SecretPropertyAccessor> GetSecretProperties(Type type)
    {
        return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Concat(type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance))
            .Where(property => property.GetCustomAttribute<SecretValueAttribute>() is not null)
            .Select(property => new SecretPropertyAccessor(
                property.Name,
                property.GetValue,
                property.GetCustomAttribute<SecretValueAttribute>()!.Keys))
            .ToArray();
    }

    private static IEnumerable<string?> NormalizeSecrets(object? value)
    {
        if (value is string || value is IEnumerable<char> || value is not IEnumerable enumerable)
        {
            yield return NormalizeSecret(value);
            yield break;
        }

        foreach (var item in enumerable)
        {
            yield return NormalizeSecret(item);
        }
    }

    private static string? NormalizeSecret(object? value)
    {
        return value switch
        {
            null => null,
            string secret => secret,
            char[] characters => new string(characters),
            Memory<char> characters => characters.ToString(),
            ReadOnlyMemory<char> characters => characters.ToString(),
            IEnumerable<char> characters => new string(characters.ToArray()),
            _ => value.ToString(),
        };
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

    private IEnumerable<string?> GetConfiguredSectionSecrets()
    {
        if (_configuration is null)
        {
            yield break;
        }

        foreach (var sectionPath in _maskingOptions.Value.MaskedConfigurationSections
                     .Where(static path => !string.IsNullOrWhiteSpace(path))
                     .Distinct(StringComparer.OrdinalIgnoreCase))
        {
            foreach (var value in GetLeafValues(_configuration.GetSection(sectionPath)))
            {
                yield return value;
            }
        }
    }

    private static IEnumerable<string?> GetLeafValues(IConfigurationSection section)
    {
        yield return section.Value;

        foreach (var child in section.GetChildren())
        {
            foreach (var value in GetLeafValues(child))
            {
                yield return value;
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
