using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.UnitTests;

public class LoggingSecretTests
{
    private class MySecretSettings
    {
        [SecretValue] public string Secret1 { get; set; } = "";
    }
    private class SecretValueLoggingModule1 : Module
    {
        private readonly IOptions<MySecretSettings> _options;

        public SecretValueLoggingModule1(IOptions<MySecretSettings> options)
        {
            _options = options;
        }

        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            context.Logger.LogInformation("My Secret Value is: {SecretValue}", _options.Value.Secret1);
            await Task.CompletedTask;
            return null;
        }
    }

    [TestCase("Shh!")]
    [TestCase("SuperSecret!")]
    [TestCase("🤐")]
    public async Task SecretIsCensored(string secretValue)
    {
        var stringBuilder = new StringBuilder();

        await TestPipelineHostBuilder.Create()
            .ConfigureServices((context, collection) =>
            {
                collection
                    .AddSingleton<ILogger<SecretValueLoggingModule1>>(new StringLogger<SecretValueLoggingModule1>(stringBuilder))
                    .AddModule<SecretValueLoggingModule1>()
                    .Configure<MySecretSettings>(settings => settings.Secret1 = secretValue);
            })
            .ExecutePipelineAsync();

        var actualLogResult = stringBuilder.ToString().Trim();

        Assert.That(actualLogResult, Does.StartWith($"My Secret Value is: **********"));
        Assert.That(actualLogResult, Does.Not.Contain(secretValue));
    }
}