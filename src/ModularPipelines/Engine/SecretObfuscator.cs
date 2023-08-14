using System.Reflection;
using System.Text;
using ModularPipelines.Attributes;

namespace ModularPipelines.Engine;

internal class SecretObfuscator : ISecretObfuscator
{
    private readonly IOptionsProvider _optionsProvider;

    private readonly string[] _secrets;

    public SecretObfuscator(IOptionsProvider optionsProvider)
    {
        _optionsProvider = optionsProvider;

        _secrets = GetSecrets(optionsProvider.GetOptions()).ToArray();
    }

    public string Obfuscate(string input, object? optionsObject)
    {
        var stringBuilder = new StringBuilder(input);

        var secretsFromExtraObject = GetSecrets(optionsObject);

        foreach (var secret in _secrets.Concat(secretsFromExtraObject))
        {
            if (input.Contains(secret))
            {
                stringBuilder.Replace(secret, new string('*', secret.Length));
            }
        }

        return stringBuilder.ToString();
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