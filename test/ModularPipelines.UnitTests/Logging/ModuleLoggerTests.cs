using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using NReco.Logging.File;
using File = ModularPipelines.FileSystem.File;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace ModularPipelines.UnitTests.Logging;

public class ModuleLoggerTests
{
    private static readonly string RandomString = Guid.NewGuid().ToString();
    private class Module1 : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            ((IConsoleWriter) context.Logger).LogToConsole(RandomString);

            ((IConsoleWriter) context.Logger).LogToConsole(new MySecrets().Value1!);

            await Task.Yield();
            return true;
        }
    }

    public class Module2 : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            context.Logger.LogInformation(new MySecrets().Value1!);

            await Task.Yield();
            return true;
        }
    }

    public class Module3 : Module<bool>
    {
        protected internal override async Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            context.Logger.LogInformation("{Value}", new MySecrets().Value1!);

            await Task.Yield();
            return true;
        }
    }

    [Test]
    public async Task LogToConsole_Does_Not_Write_To_File_Logger()
    {
        // This test verifies that LogToConsole output goes to console buffers,
        // NOT to file loggers. The console output itself is verified implicitly
        // through the full integration tests.
        var file = File.GetNewTemporaryFilePath();

        var host = await TestPipelineHostBuilder.Create()
            .ConfigureServices((_, collection) =>
            {
                collection.AddLogging(builder => { builder.AddFile(file); });
            })
            .AddModule<Module1>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        await host.DisposeAsync();

        // The key behavior: LogToConsole output should NOT appear in file logs
        await Assert.That(await file.ReadAsync()).DoesNotContain(RandomString);
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
                collection.AddSingleton(typeof(IModule), moduleType);
            })
            .SetLogLevel(LogLevel.Information)
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        await host.DisposeAsync();

        await Assert.That(await file.ReadAsync()).DoesNotContain("Secret Value!!!");
        await Assert.That(await file.ReadAsync()).Contains("**********");
    }

    private class MySecrets
    {
        [SecretValue] public string? Value1 { get; init; } = "Secret Value!!!";
    }
}
