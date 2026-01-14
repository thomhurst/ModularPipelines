using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using Initialization.Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
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
    private static readonly ConcurrentDictionary<Type, PropertyInfo[]> SecretPropertiesCache = new();
    private static readonly ConcurrentDictionary<PropertyInfo, Func<object, object?>> PropertyGettersCache = new();

    private readonly IOptionsProvider _optionsProvider;
    private readonly IBuildSystemSecretMasker _buildSystemSecretMasker;
    private readonly IOptions<SecretMaskingOptions> _maskingOptions;
    private readonly ConcurrentDictionary<string, byte> _secrets = new();
    private readonly object _initLock = new();

    private bool _initialized;

    public IEnumerable<string> Secrets => _secrets.Keys;

    public SecretProvider(
        IOptionsProvider optionsProvider,
        IBuildSystemSecretMasker buildSystemSecretMasker,
        IOptions<SecretMaskingOptions> maskingOptions)
    {
        _optionsProvider = optionsProvider;
        _buildSystemSecretMasker = buildSystemSecretMasker;
        _maskingOptions = maskingOptions;
    }

    /// <inheritdoc />
    public void AddSecret(string? secret)
    {
        if (string.IsNullOrWhiteSpace(secret))
        {
            return;
        }

        var options = _maskingOptions.Value;

        // Check minimum length requirement
        if (secret.Length < options.MinimumSecretLength)
        {
            return;
        }

        // TryAdd returns false if already exists, providing thread-safe deduplication
        if (_secrets.TryAdd(secret, 0))
        {
            // Register with build system for native masking (only for new secrets)
            _buildSystemSecretMasker.MaskSecrets([secret]);
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
        AddSecrets((IEnumerable<string?>)secrets);
    }

    public IEnumerable<string> GetSecretsInObject(object? value)
    {
        if (value is null)
        {
            yield break;
        }

        var type = value.GetType();
        var secretProperties = SecretPropertiesCache.GetOrAdd(type, GetSecretProperties);
        var options = _maskingOptions.Value;

        foreach (var property in secretProperties)
        {
            var getter = PropertyGettersCache.GetOrAdd(property, CreatePropertyGetter);
            var secret = getter(value)?.ToString();

            if (!string.IsNullOrWhiteSpace(secret) && secret.Length >= options.MinimumSecretLength)
            {
                yield return secret;
            }
        }
    }

    public Task InitializeAsync()
    {
        lock (_initLock)
        {
            if (_initialized)
            {
                return Task.CompletedTask;
            }

            var secretsFromOptions = GetSecrets(_optionsProvider.GetOptions());
            foreach (var secret in secretsFromOptions)
            {
                _secrets.TryAdd(secret, 0);
            }

            _initialized = true;
        }

        return Task.CompletedTask;
    }

    private static PropertyInfo[] GetSecretProperties(Type type)
    {
        return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Concat(type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance))
            .Where(m => m.GetCustomAttribute<SecretValueAttribute>() is not null)
            .ToArray();
    }

    private static Func<object, object?> CreatePropertyGetter(PropertyInfo property)
    {
        var instanceParam = Expression.Parameter(typeof(object), "instance");
        var castInstance = Expression.Convert(instanceParam, property.DeclaringType!);
        var propertyAccess = Expression.Property(castInstance, property);
        var castResult = Expression.Convert(propertyAccess, typeof(object));
        var lambda = Expression.Lambda<Func<object, object?>>(castResult, instanceParam);
        return lambda.Compile();
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
}
