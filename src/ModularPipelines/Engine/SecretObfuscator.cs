using System.Reflection;
using System.Text;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using TomLonghurst.Microsoft.Extensions.DependencyInjection.ServiceInitialization;

namespace ModularPipelines.Engine;

internal class SecretObfuscator : ISecretObfuscator, IInitializer
{
    private readonly IBuildSystemDetector _buildSystemDetector;

    private readonly string[] _secrets;

    public SecretObfuscator(IOptionsProvider optionsProvider, 
        IBuildSystemDetector buildSystemDetector)
    {
        _buildSystemDetector = buildSystemDetector;

        _secrets = GetSecrets(optionsProvider.GetOptions()).ToArray();
        InitializeAsync();
    }

    public string Obfuscate(string input, object? optionsObject)
    {
        var stringBuilder = new StringBuilder(input);

        var secretsFromExtraObject = GetSecrets(optionsObject);

        foreach (var secret in _secrets.Concat(secretsFromExtraObject))
        {
            if (input.Contains(secret))
            {
                stringBuilder.Replace(secret, "**********");
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

    public Task InitializeAsync()
    {
        foreach (var secret in _secrets)
        {
            if (_buildSystemDetector.IsRunningOnGitHubActions)
            {
                Console.WriteLine($@"::add-mask::{secret}");
            }
        }
        
        return Task.CompletedTask;
    }
}