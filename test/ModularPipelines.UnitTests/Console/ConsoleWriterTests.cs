using ModularPipelines;
using ModularPipelines.Attributes.Events;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.Interfaces;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
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

    private sealed class WriteInvalidMarkupModule(IConsoleWriter consoleWriter) : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            consoleWriter.WriteMarkupLine("[green]unclosed");
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

    private sealed class WriteSplitSecretModule(
        IConsoleWriter consoleWriter,
        ISecretRegistry secretRegistry) : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            secretRegistry.AddSecret("abc123");
            consoleWriter.WriteMarkupLine("[red]abc[/][blue]123[/]");
            return Task.FromResult(true);
        }
    }

    [WriteReadyOutput]
    private sealed class ReadyOutputModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(true);
    }

    private sealed class PlanningOutputModule : Module<bool>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithSkipWhen(context =>
            {
                context.Console.WriteLine("planning condition output");
                return SkipDecision.DoNotSkip;
            });

        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult(true);
    }

    [AttributeUsage(AttributeTargets.Class)]
    private sealed class WriteReadyOutputAttribute : Attribute, IModuleReadyHandler
    {
        public Task OnModuleReadyAsync(IModuleHookContext context)
        {
            context.Console.WriteLine("attribute ready output");
            return Task.CompletedTask;
        }
    }

    private sealed class WriteLifecycleOutputReceiver : IModuleEventReceiver
    {
        public Task OnModuleReadyAsync(IModuleHookContext context)
        {
            context.Console.WriteLine("receiver ready output");
            return Task.CompletedTask;
        }

        public Task OnModuleStartAsync(IModuleHookContext context)
        {
            context.Console.WriteLine("receiver start output");
            return Task.CompletedTask;
        }

        public Task OnModuleEndAsync(IModuleHookContext context)
        {
            context.Console.WriteLine("receiver end output");
            return Task.CompletedTask;
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

        await Assert.That(output).Contains("[green]module output[/]");
    }

    [Test]
    public async Task WriteMarkupLine_InvalidMarkupFallsBackToPlainModuleOutput()
    {
        var output = await RunAsync<WriteInvalidMarkupModule>();

        await Assert.That(output).Contains("[green]unclosed");
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
    public async Task WriteMarkupLine_InvalidMarkupUsesConfiguredConsole()
    {
        var output = CaptureFallbackOutput(writer => writer.WriteMarkupLine("[green]unclosed"));

        await Assert.That(output).Contains("[green]unclosed");
    }

    [Test]
    public async Task WriteMarkupLine_ObfuscatesMarkupWrappedSecret()
    {
        var output = CaptureFallbackOutput(
            writer => writer.WriteMarkupLine("[red]secret[/]"),
            CreateSecretObfuscator("[red]secret[/]"));

        await Assert.That(output).Contains("**********");
        await Assert.That(output).DoesNotContain("secret");
    }

    [Test]
    public async Task Write_ObfuscatesWithoutAmbientModule()
    {
        var output = CaptureFallbackOutput(writer => writer.Write(new Markup("[green]a secret value[/]")));

        await AssertFallbackOutputIsObfuscated(output);
    }

    [Test]
    public async Task WriteMarkupLine_PreservesTagsThatMatchSecrets()
    {
        var output = CaptureFallbackOutput(
            writer => writer.WriteMarkupLine("[green]visible[/]"),
            CreateSecretObfuscator("green"));

        await Assert.That(output).Contains("visible");
        await Assert.That(output).DoesNotContain("**********");
    }

    [Test]
    public async Task WriteMarkupLine_ObfuscatesSecretSplitAcrossTags()
    {
        var output = CaptureFallbackOutput(
            writer => writer.WriteMarkupLine("[red]abc[/][blue]123[/]"),
            CreateSecretObfuscator("abc123"));

        await Assert.That(output).Contains("**********");
        await Assert.That(output).DoesNotContain("abc");
        await Assert.That(output).DoesNotContain("123");
    }

    [Test]
    public async Task Write_PreservesRenderableStylesWithoutAmbientModule()
    {
        var output = CaptureFallbackOutput(
            writer => writer.Write(new Markup("[red]styled[/]")),
            CreateSecretObfuscator("unrelated"),
            AnsiSupport.Yes);

        await Assert.That(output).Contains("\u001b[");
        await Assert.That(output).Contains("styled");
    }

    [Test]
    public async Task Write_CustomObfuscatorPreservesSegmentStyles()
    {
        var secretObfuscator = new Mock<ISecretObfuscator>();
        secretObfuscator
            .Setup(x => x.Obfuscate(It.IsAny<string?>(), null))
            .Returns("masked");

        var output = CaptureFallbackOutput(
            writer => writer.Write(new Markup("[red]abc[/][blue]123[/]")),
            secretObfuscator.Object,
            AnsiSupport.Yes);

        using (Assert.Multiple())
        {
            await Assert.That(output).Contains("masked");
            await Assert.That(output).Contains("\u001b[");
            await Assert.That(output.Split("masked").Length - 1).IsEqualTo(2);
        }
    }

    [Test]
    public async Task Write_ObfuscatesSecretInHyperlinkTarget()
    {
        var renderable = new SecretObfuscatedRenderable(
            new Markup("[link=https://example.test/token]label[/]"),
            CreateSecretObfuscator("token"));
        var segments = renderable.Render(
                RenderOptions.Create(AnsiConsole.Console),
                80)
            .ToArray();

        var url = segments.Select(static segment => segment.Link?.Url)
            .First(static value => value is not null)!;
        await Assert.That(url).Contains("**********");
        await Assert.That(url).DoesNotContain("token");
    }

    [Test]
    public async Task Write_MeasuresObfuscatedWidth()
    {
        var renderable = new SecretObfuscatedRenderable(
            new Text("tiny"),
            CreateSecretObfuscator("tiny"));

        var measurement = renderable.Measure(
            RenderOptions.Create(AnsiConsole.Console),
            80);

        await Assert.That(measurement.Max).IsEqualTo(10);
    }

    [Test]
    public async Task Write_DoesNotPreserveMaskContainingSecret()
    {
        var secretProvider = new Mock<ISecretProvider>();
        secretProvider.SetupGet(x => x.Version).Returns(0);
        secretProvider.Setup(x => x.GetSnapshot())
            .Returns(new SecretSnapshot(0, ["REDACT"]));
        var obfuscator = new SecretObfuscator(
            secretProvider.Object,
            Microsoft.Extensions.Options.Options.Create(new SecretMaskingOptions
            {
                MaskValue = "[REDACTED]",
            }));
        var output = CaptureFallbackOutput(
            writer => writer.Write(new Text("[REDACTED]")),
            obfuscator);

        await Assert.That(output).DoesNotContain("REDACT");
    }

    [Test]
    public async Task WriteMarkupLine_ObfuscatesSplitSecretInModuleBuffer()
    {
        var output = await RunAsync<WriteSplitSecretModule>();

        await Assert.That(output).Contains("**********");
        await Assert.That(output).DoesNotContain("abc");
        await Assert.That(output).DoesNotContain("123");
    }

    [Test]
    public async Task LifecycleHooks_UseModuleConsoleWriter()
    {
        var output = await RunAsync<ReadyOutputModule>(
            builder => builder.AddModuleEventReceiver<WriteLifecycleOutputReceiver>());

        await Assert.That(output).Contains("attribute ready output");
        await Assert.That(output).Contains("receiver ready output");
        await Assert.That(output).Contains("receiver start output");
        await Assert.That(output).Contains("receiver end output");
    }

    [Test]
    public async Task PlanningCondition_UsesModuleConsoleWriter()
    {
        var output = await RunAsync<PlanningOutputModule>();

        await Assert.That(output).Contains("planning condition output");
    }

    private static async Task<string> RunAsync<TModule>(
        Action<PipelineBuilder>? configure = null)
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
        configure?.Invoke(builder);

        var summary = await builder.RunAsync();

        return summary.RunReport!.Modules.Single().Output!.StdoutTail ?? string.Empty;
    }

    private static string CaptureFallbackOutput(
        Action<ConsoleWriter> write,
        ISecretObfuscator? secretObfuscator = null,
        AnsiSupport ansiSupport = AnsiSupport.No)
    {
        var originalConsole = AnsiConsole.Console;
        using var output = new StringWriter();

        try
        {
            AnsiConsole.Console = AnsiConsole.Create(new AnsiConsoleSettings
            {
                Out = new AnsiConsoleOutput(output),
                Ansi = ansiSupport,
                ColorSystem = ansiSupport is AnsiSupport.Yes
                    ? ColorSystemSupport.Standard
                    : ColorSystemSupport.NoColors,
            });

            secretObfuscator ??= CreateMockSecretObfuscator();

            write(new ConsoleWriter(secretObfuscator));
            return output.ToString();
        }
        finally
        {
            AnsiConsole.Console = originalConsole;
        }
    }

    private static ISecretObfuscator CreateMockSecretObfuscator()
    {
        var secretObfuscator = new Mock<ISecretObfuscator>();
        secretObfuscator
            .Setup(x => x.Obfuscate(It.IsAny<string?>(), null))
            .Returns((string? input, object? _) => input!.Replace("secret", "********"));
        return secretObfuscator.Object;
    }

    private static ISecretObfuscator CreateSecretObfuscator(params string[] secrets)
    {
        var secretProvider = new Mock<ISecretProvider>();
        secretProvider.SetupGet(x => x.Version).Returns(0);
        secretProvider.Setup(x => x.GetSnapshot()).Returns(new SecretSnapshot(0, secrets));
        return new SecretObfuscator(
            secretProvider.Object,
            Microsoft.Extensions.Options.Options.Create(new SecretMaskingOptions()));
    }

    private static async Task AssertFallbackOutputIsObfuscated(string output)
    {
        await Assert.That(output).Contains("********");
        await Assert.That(output).DoesNotContain("secret");
    }
}
