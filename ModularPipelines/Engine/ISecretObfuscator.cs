using System.Reflection;
using System.Text;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.DependencyInjection;

namespace ModularPipelines.Engine;

public interface ISecretObfuscator
{
    string Obfuscate(string input);
}

internal class SecretObfuscator : ISecretObfuscator
{
    private readonly IOptionsProvider _optionsProvider;
    
    private readonly string[] _secrets;

    public SecretObfuscator(IOptionsProvider optionsProvider)
    {
        _optionsProvider = optionsProvider;

        _secrets = GetSecrets(optionsProvider.GetOptions()).ToArray();
    }
    
    public string Obfuscate(string input)
    {
        var stringBuilder = new StringBuilder(input);
        
        foreach (var secret in _secrets)
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
}

public interface IOptionsProvider
{
    IEnumerable<object?> GetOptions();
}

internal class OptionsProvider : IOptionsProvider
{
    private readonly IPipelineServiceContainerWrapper _pipelineServiceContainerWrapper;
    private readonly IServiceProvider _serviceProvider;

    public OptionsProvider(IPipelineServiceContainerWrapper pipelineServiceContainerWrapper, IServiceProvider serviceProvider)
    {
        _pipelineServiceContainerWrapper = pipelineServiceContainerWrapper;
        _serviceProvider = serviceProvider;
    }

    public IEnumerable<object?> GetOptions()
    {
        var types = _pipelineServiceContainerWrapper.ServiceCollection
            .Select(sd => sd.ServiceType)
            .Where(t => t.IsGenericType)
            .Where(t => t.GetGenericTypeDefinition().IsAssignableTo(typeof(IConfigureOptions<>)) || t.GetGenericTypeDefinition().IsAssignableTo(typeof(IPostConfigureOptions<>)))
            .Select(s => s.GetGenericArguments()[0])
            .ToList();

        foreach (var option in types.Select(t => _serviceProvider.GetService(typeof(IOptions<>).MakeGenericType(new Type[] { t }))))
        {
            yield return option.GetType().GetProperty("Value", BindingFlags.Public | BindingFlags.Instance).GetValue(option);
        }
    }
}