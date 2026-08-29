using ModularPipelines.Logging;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Logging;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using Moq;
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

    [Test]
    public async Task WriteLine_ObfuscatesWithoutAmbientModule()
    {
        var output = CaptureFallbackOutput(writer => writer.WriteLine("a secret value"));

        await AssertFallbackOutputIsObfuscated(output);
    }

    [Test]
    public async Task WriteMarkupLine_ObfuscatesWithoutAmbientModule()
    {
        var output = CaptureFallbackOutput(writer => writer.WriteMarkupLine("[green]a secret value[/]"));

        await AssertFallbackOutputIsObfuscated(output);
    }

    [Test]
    public async Task Write_ObfuscatesWithoutAmbientModule()
    {
        var output = CaptureFallbackOutput(writer => writer.Write(new Markup("[green]a secret value[/]")));

        await AssertFallbackOutputIsObfuscated(output);
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

    private static string CaptureFallbackOutput(Action<ConsoleWriter> write)
    {
        var originalConsole = AnsiConsole.Console;
        using var output = new StringWriter();

        try
        {
            AnsiConsole.Console = AnsiConsole.Create(new AnsiConsoleSettings
            {
                Out = new AnsiConsoleOutput(output),
                Ansi = AnsiSupport.No,
            });

            var secretObfuscator = new Mock<ISecretObfuscator>();
            secretObfuscator
                .Setup(x => x.Obfuscate(It.IsAny<string?>(), null))
                .Returns((string? input, object? _) => input!.Replace("secret", "********"));

            write(new ConsoleWriter(secretObfuscator.Object));
            return output.ToString();
        }
        finally
        {
            AnsiConsole.Console = originalConsole;
        }
    }

    private static async Task AssertFallbackOutputIsObfuscated(string output)
    {
        await Assert.That(output).Contains("********");
        await Assert.That(output).DoesNotContain("secret");
    }
}
