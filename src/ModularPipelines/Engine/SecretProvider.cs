using System.Reflection;
using ModularPipelines.Attributes;
using TomLonghurst.Microsoft.Extensions.DependencyInjection.ServiceInitialization;

namespace ModularPipelines.Engine;

internal class SecretProvider : ISecretProvider
{
    private readonly IOptionsProvider _optionsProvider;
    private readonly HashSet<string> _cachedSecrets = new();
    public IEnumerable<string> Secrets => _cachedSecrets.Concat(GetSecrets(_optionsProvider.GetOptions())).Distinct();

    public SecretProvider(IOptionsProvider optionsProvider)
    {
        _optionsProvider = optionsProvider;
    }

    private IEnumerable<string> GetSecrets(IEnumerable<object?> options)
    {
        foreach (var option in options)
        {
            foreach (var secret in GetSecretsInObject(option))
            {
                _cachedSecrets.Add(secret);
                yield return secret;
            }
        }
    }

    public IEnumerable<string> GetSecretsInObject(object? option)
    {
        if (option is null)
        {
            yield break;
        }

        foreach (var secretValueMember in option.GetType()
                     .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                     .Concat(option.GetType()
                         .GetProperties(BindingFlags.NonPublic | BindingFlags.Instance))
                     .Where(m => m.GetCustomAttribute<SecretValueAttribute>() is not null))
        {
            var secret = secretValueMember.GetValue(option)?.ToString();

            if (!string.IsNullOrWhiteSpace(secret))
            {
                yield return secret;
            }
        }
    }
}