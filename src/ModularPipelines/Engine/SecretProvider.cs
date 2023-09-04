using System.Reflection;
using ModularPipelines.Attributes;

namespace ModularPipelines.Engine;

internal class SecretProvider : ISecretProvider
{
    public string[] Secrets { get; }

    public SecretProvider(IOptionsProvider optionsProvider)
    {
        Secrets = GetSecrets(optionsProvider.GetOptions()).ToArray();
    }

    private IEnumerable<string> GetSecrets(IEnumerable<object?> options)
    {
        foreach (var option in options)
        {
            foreach (var secret in GetSecrets(option))
            {
                yield return secret;
            }
        }
    }

    public IEnumerable<string> GetSecrets(object? option)
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