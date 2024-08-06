using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using NReco.Logging.File;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.UnitTests;

public class ModuleLoggerTests
{
    private static readonly string RandomString = Guid.NewGuid().ToString();
    private class Module1 : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            context.Logger.LogToConsole(RandomString);

            context.Logger.LogToConsole(new MySecrets().Value1!);

            return await NothingAsync();
        }
    }

    public class Module2 : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            context.Logger.LogInformation(new MySecrets().Value1!);

            return await NothingAsync();
        }
    }
    
    public class Module3 : Module
    {
        protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            context.Logger.LogInformation("{Value}", new MySecrets().Value1!);

            return await NothingAsync();
        }
    }

    [Test]
    public async Task Can_Write_To_Console_Successfully()
    {
        var consoleStringBuilder = new StringBuilder();
        var file = File.GetNewTemporaryFilePath();

        var host = await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, collection) =>
            {
                collection.AddLogging(builder => { builder.AddFile(file); });
                collection.AddSingleton<IConsoleWriter>(new StringBuilderConsoleWriter(consoleStringBuilder));
            })
            .AddModule<Module1>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        await host.DisposeAsync();

        var stringOutput = consoleStringBuilder.ToString();
        await Assert.That(stringOutput).Does.Contain(RandomString);
        await Assert.That(await file.ReadAsync()).Does.Not.Contain(RandomString);
    }
    
    [Test]
    [Arguments(typeof(Module2))]
    [Arguments(typeof(Module3))]
    public async Task Can_Obfuscate_Secret(Type moduleType)
    {
        var file = File.GetNewTemporaryFilePath();

        var host = await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, collection) =>
            {
                collection.Configure<MySecrets>(_.Configuration);
                collection.AddLogging(builder => { builder.AddFile(file); });
                collection.AddSingleton(typeof(ModuleBase), moduleType);
            })
            .SetLogLevel(LogLevel.Information)
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        await host.DisposeAsync();

        await Assert.That(await file.ReadAsync()).Does.Not.Contain("Secret Value!!!");
        await Assert.That(await file.ReadAsync()).Does.Contain("**********");
    }

    private class MySecrets
    {
        [SecretValue] public string? Value1 { get; init; } = "Secret Value!!!";
    }
}