using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Initialization.Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
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
/// <item>Leaf values beneath configured <see cref="SecretMaskingOptions.MaskedConfigurationSections"/> paths</item>
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
        Justification = "Generated secret accessors handle statically known option types; GetSecretProperties is the documented reflection fallback for dynamic options.")]
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
            if (secretValueKeys.Any(secretKey => IsMatchingSecretKey(keyValue.Key, secretKey)) &&
                !string.IsNullOrWhiteSpace(keyValue.Value))
            {
                yield return keyValue.Value;
            }
        }
    }

    private static bool IsMatchingSecretKey(string key, string secretKey)
    {
        if (string.Equals(key, secretKey, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        var keySegments = SplitKeySegments(key);
        var secretSegments = SplitKeySegments(secretKey);
        if (secretSegments.Count == 0 || secretSegments.Count > keySegments.Count)
        {
            return false;
        }

        for (var start = 0; start <= keySegments.Count - secretSegments.Count; start++)
        {
            var allSegmentsMatch = true;
            for (var offset = 0; offset < secretSegments.Count; offset++)
            {
                if (!string.Equals(
                        secretSegments[offset],
                        keySegments[start + offset],
                        StringComparison.OrdinalIgnoreCase))
                {
                    allSegmentsMatch = false;
                    break;
                }
            }

            if (allSegmentsMatch)
            {
                return true;
            }
        }

        return false;
    }

    private static IReadOnlyList<string> SplitKeySegments(string key)
    {
        var segments = new List<string>();
        var segmentStart = 0;
        for (var index = 0; index < key.Length; index++)
        {
            if (key[index] is '.' or '_' or '-')
            {
                AddSegment(key, segmentStart, index, segments);
                segmentStart = index + 1;
                continue;
            }

            if (index > segmentStart
                && char.IsUpper(key[index])
                && (char.IsLower(key[index - 1])
                    || (char.IsUpper(key[index - 1])
                        && index + 1 < key.Length
                        && char.IsLower(key[index + 1]))))
            {
                AddSegment(key, segmentStart, index, segments);
                segmentStart = index;
            }
        }

        AddSegment(key, segmentStart, key.Length, segments);
        return segments;
    }

    private static void AddSegment(
        string key,
        int start,
        int end,
        ICollection<string> segments)
    {
        if (end > start)
        {
            segments.Add(key[start..end]);
        }
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
            CliOptionValue optionValue => optionValue.Value,
            _ => value.ToString(),
        };
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
