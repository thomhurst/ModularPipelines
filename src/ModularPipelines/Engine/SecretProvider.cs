using System.Reflection;
using ModularPipelines.Attributes;
using TomLonghurst.Microsoft.Extensions.DependencyInjection.ServiceInitialization;

namespace ModularPipelines.Engine;

internal class SecretProvider : ISecretProvider, IInitializer
{
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

        foreach (var secretValueMember in value.GetType()
                     .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                     .Concat(value.GetType()
                         .GetProperties(BindingFlags.NonPublic | BindingFlags.Instance))
                     .Where(m => m.GetCustomAttribute<SecretValueAttribute>() is not null))
        {
            var secret = secretValueMember.GetValue(value)?.ToString();

            if (!string.IsNullOrWhiteSpace(secret))
            {
                yield return secret;
            }
        }
    }

    public Task InitializeAsync()
    {
        _secrets = GetSecrets(_optionsProvider.GetOptions()).ToHashSet();
        return Task.CompletedTask;
    }

    private IEnumerable<string> GetSecrets(IEnumerable<object?> options)
    {
        foreach (var option in options)
        {
            foreach (var secret in GetSecretsInObject(option))
            {
                _secrets.Add(secret);
                yield return secret;
            }
        }
    }
}