using ModularPipelines.Logging;
using ModularPipelines.Context;
using ModularPipelines.Logging;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ModularPipelines.UnitTests.Console;

[TUnit.Core.NotInParallel(nameof(ConsoleWriterTests))]
public class ConsoleWriterTests
{
    private sealed class WriteMarkupLineModule(IConsoleWriter consoleWriter) : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            consoleWriter.WriteMarkupLine("[green]module output[/]");
            return Task.FromResult(true);
        }
    }

    private sealed class WriteLineModule(IConsoleWriter consoleWriter) : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            consoleWriter.WriteLine("[green]module output[/]");
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
    public async Task WriteMarkupLine_UsesAmbientModuleConsoleWriter()
    {
        var output = await RunAsync<WriteMarkupLineModule>();

        await Assert.That(output).Contains("module output");
    }

    [Test]
    public async Task WriteLine_EscapesMarkup()
    {
        var output = await RunAsync<WriteLineModule>();

        await Assert.That(output).Contains("[[green]]module output[[/]]");
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
        var builder = TestPipelineBuilder.Create();
        builder.ConfigureOptions(options => options with
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
