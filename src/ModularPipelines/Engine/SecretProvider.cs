using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;
using Initialization.Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;

namespace ModularPipelines.Engine;

internal class SecretProvider : ISecretProvider, IInitializer
{
    private static readonly ConcurrentDictionary<Type, PropertyInfo[]> SecretPropertiesCache = new();
    private static readonly ConcurrentDictionary<PropertyInfo, Func<object, object?>> PropertyGettersCache = new();

    private readonly IOptionsProvider _optionsProvider;

    private HashSet<string> _secrets = new();

    public IEnumerable<string> Secrets => _secrets;

    public SecretProvider(IOptionsProvider optionsProvider)
    {
        _optionsProvider = optionsProvider;
    }

    public IEnumerable<string> GetSecretsInObject(object? value)
    {
        if (value is null)
        {
            yield break;
        }

        var type = value.GetType();
        var secretProperties = SecretPropertiesCache.GetOrAdd(type, GetSecretProperties);

        foreach (var property in secretProperties)
        {
            var getter = PropertyGettersCache.GetOrAdd(property, CreatePropertyGetter);
            var secret = getter(value)?.ToString();

            if (!string.IsNullOrWhiteSpace(secret))
            {
                yield return secret;
            }
        }
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

    public Task InitializeAsync()
    {
        _secrets = GetSecrets(_optionsProvider.GetOptions()).ToHashSet();
        return Task.CompletedTask;
    }

    private IEnumerable<string> GetSecrets(IEnumerable<object?> options)
    {
        var secrets = new List<string>();

        foreach (var option in options)
        {
            foreach (var secret in GetSecretsInObject(option))
            {
                secrets.Add(secret);
            }
        }

        return secrets;
    }
}