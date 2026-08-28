using ModularPipelines.Context;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ModularPipelines.UnitTests.Console;

[TUnit.Core.NotInParallel(nameof(ConsoleWriterTests))]
public class ConsoleWriterTests
{
    private sealed class LogToConsoleModule(IConsoleWriter consoleWriter) : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            consoleWriter.LogToConsole("[green]module output[/]");
            return Task.FromResult(true);
        }
    }

    private sealed class WriteModule(IConsoleWriter consoleWriter) : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            IRenderable table = new Table().AddColumn("Value").AddRow("module output");
            consoleWriter.Write(table);
            return Task.FromResult(true);
        }
    }

    [Test]
    public async Task LogToConsole_UsesAmbientModuleConsoleWriter()
    {
        var output = await RunAsync<LogToConsoleModule>();

        await Assert.That(output).Contains("module output");
    }

    [Test]
    public async Task Write_UsesAmbientModuleConsoleWriter()
    {
        var output = await RunAsync<WriteModule>();

        await Assert.That(output).Contains("module output");
    }

    private static async Task<string> RunAsync<TModule>()
        where TModule : class, IModule
    {
        using var builder = TestPipelineBuilder.Create();
        builder.ConfigurePipelineOptions(options => options with
        {
            RunReport = options.RunReport with
            {
                IncludeModuleOutput = true,
                MaxOutputBytesPerModule = 1024,
            },
        });
        builder.AddModule<TModule>();

        var summary = await builder.RunAsync();

        return summary.RunReport!.Modules.Single().Output!.StdoutTail ?? string.Empty;
    }
}
