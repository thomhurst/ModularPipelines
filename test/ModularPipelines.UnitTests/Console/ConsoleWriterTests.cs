using System.Globalization;
using ModularPipelines;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Events;
using ModularPipelines.Extensions;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.Secrets;
using ModularPipelines.TestHelpers;
using Moq;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ModularPipelines.UnitTests.Console;

[TUnit.Core.NotInParallel]
public class ConsoleWriterTests
{
    private sealed class TrackingSecretProvider : ISecretProvider, ISecretEmissionGuard
    {
        private int _executionDepth;

        public int ExecutionCount { get; private set; }

        public bool IsExecuting => _executionDepth > 0;

        public long Version => 0;

        public IEnumerable<string> Secrets => [];

        public SecretSnapshot GetSnapshot() => new(0, []);

        public bool TryExecuteIfVersionCurrent(long expectedVersion, Action action)
        {
            action();
            return true;
        }

        public IEnumerable<string> GetSecretsInObject(object? value) => [];

        public void ExecuteWithStableSecrets<TState>(TState state, Action<TState> processOutput)
        {
            ExecutionCount++;
            _executionDepth++;
            try
            {
                processOutput(state);
            }
            finally
            {
                _executionDepth--;
            }
        }
    }

    private sealed class ControlRenderable(params string[] values) : IRenderable
    {
        public Measurement Measure(RenderOptions options, int maxWidth) => new(0, 0);

        public IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
        {
            foreach (var value in values)
            {
                yield return Segment.Control(value);
            }
        }
    }

    private sealed class WidthSensitiveRenderable : IRenderable
    {
        public List<int> RenderWidths { get; } = [];

        public Measurement Measure(RenderOptions options, int maxWidth) => new(1, maxWidth);

        public IEnumerable<Segment> Render(RenderOptions options, int maxWidth)
        {
            RenderWidths.Add(maxWidth);
            yield return new Segment(maxWidth.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
    }

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

    private sealed class WriteFragmentsModule : Module<bool>
    {
        protected internal override Task<bool> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            context.Console.Write(new Text("prefix"));
            context.Console.Write(new Text("suffix"));
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

    private sealed class WriteLifecycleOutputHandler : IModuleEventHandler
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

        public Task OnModuleEndAsync(IModuleHookContext context, IModuleResult result)
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
    public async Task Write_UsesAmbientModuleConsoleWriterWithoutAppendingLineBreaks()
    {
        var output = await RunAsync<WriteFragmentsModule>();

        using (Assert.Multiple())
        {
            await Assert.That(output).Contains("prefixsuffix");
            await Assert.That(output).DoesNotContain($"prefix{Environment.NewLine}suffix");
        }
    }

    [Test]
    public async Task WriteLine_ObfuscatesWithoutAmbientModule()
    {
        var output = CaptureFallbackOutput(writer => writer.WriteLine("a secret value"));

        await AssertFallbackOutputIsObfuscated(output);
    }

    [Test]
    public async Task Direct_Writes_Obfuscate_And_Emit_With_Stable_Secrets()
    {
        var secretProvider = new TrackingSecretProvider();
        var guardedObfuscations = new List<bool>();
        var secretObfuscator = new Mock<ISecretObfuscator>();
        secretObfuscator
            .Setup(x => x.Obfuscate(It.IsAny<string?>(), null))
            .Returns((string? input, object? _) =>
            {
                guardedObfuscations.Add(secretProvider.IsExecuting);
                return input ?? string.Empty;
            });

        CaptureFallbackOutput(
            writer =>
            {
                writer.WriteLine("plain text");
                writer.WriteMarkupLine("[green]markup text[/]");
                writer.Write(new Text("renderable text"));
            },
            secretObfuscator.Object,
            secretProvider: secretProvider);

        using (Assert.Multiple())
        {
            await Assert.That(secretProvider.ExecutionCount).IsEqualTo(3);
            await Assert.That(guardedObfuscations).IsNotEmpty();
            await Assert.That(guardedObfuscations.All(static isGuarded => isGuarded)).IsTrue();
        }
    }

    [Test]
    public async Task WriteLine_UsesInjectedConsoleWithoutAmbientModule()
    {
        var originalConsole = AnsiConsole.Console;
        using var globalOutput = new StringWriter();
        using var injectedOutput = new StringWriter();
        try
        {
            AnsiConsole.Console = CreateConsole(globalOutput);
            var injectedConsole = CreateConsole(injectedOutput);

            new ConsoleWriter(
                    CreateMockSecretObfuscator(),
                    new Mock<ISecretProvider>().Object,
                    injectedConsole)
                .WriteLine("injected output");

            using (Assert.Multiple())
            {
                await Assert.That(injectedOutput.ToString()).Contains("injected output");
                await Assert.That(globalOutput.ToString()).DoesNotContain("injected output");
            }
        }
        finally
        {
            AnsiConsole.Console = originalConsole;
        }
    }

    [Test]
    public async Task WriteMarkupLine_ObfuscatesWithoutAmbientModule()
    {
        var output = CaptureFallbackOutput(writer => writer.WriteMarkupLine("[green]a secret value[/]"));

        await AssertFallbackOutputIsObfuscated(output);
    }

    [Test]
    public async Task WriteMarkupLine_DoesNotApplyCustomObfuscatorTwice()
    {
        var secretObfuscator = new Mock<ISecretObfuscator>();
        secretObfuscator
            .Setup(x => x.Obfuscate(It.IsAny<string?>(), null))
            .Returns((string? input, object? _) => input switch
            {
                "secret" => "masked",
                "masked" => "masked-twice",
                _ => input ?? string.Empty,
            });

        var output = CaptureFallbackOutput(
            writer => writer.WriteMarkupLine("[green]secret[/]"),
            secretObfuscator.Object);

        await Assert.That(output).Contains("masked");
        await Assert.That(output).DoesNotContain("masked-twice");
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
    public async Task WriteMarkupLine_InvalidObfuscatedMarkupNeverFallsBackToRawSecret()
    {
        var secretProvider = new Mock<ISecretProvider>();
        secretProvider.SetupGet(x => x.Version).Returns(0);
        secretProvider.Setup(x => x.GetSnapshot())
            .Returns(new SecretSnapshot(0, ["[red]secret[/]"]));
        var obfuscator = new SecretObfuscator(
            secretProvider.Object,
            Microsoft.Extensions.Options.Options.Create(new SecretMaskingOptions
            {
                MaskValue = "[REDACTED]",
            }));

        var output = CaptureFallbackOutput(
            writer => writer.WriteMarkupLine("value: [red]secret[/]"),
            obfuscator);

        await Assert.That(output).Contains("[REDACTED]");
        await Assert.That(output).DoesNotContain("secret");
    }

    [Test]
    public async Task WriteMarkupLine_EscapesBracketedMaskWithoutDroppingMarkup()
    {
        var output = CaptureFallbackOutput(
            writer => writer.WriteMarkupLine("[red]secret[/]"),
            CreateSecretObfuscator("secret", "[REDACTED]"),
            AnsiSupport.Yes);

        using (Assert.Multiple())
        {
            await Assert.That(output).Contains("\u001b[");
            await Assert.That(output).Contains("[REDACTED]");
            await Assert.That(output).DoesNotContain("secret");
        }
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

        await Assert.That(output).Contains("******");
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

        var renderable = new SecretObfuscatedRenderable(
            new Markup("[red]abc[/][blue]123[/]"),
            secretObfuscator.Object);
        var segments = renderable.Render(
                RenderOptions.Create(AnsiConsole.Console),
                80)
            .Where(static segment => !segment.IsControlCode)
            .ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(string.Concat(segments.Select(static segment => segment.Text)))
                .IsEqualTo("masked");
            await Assert.That(segments).Count().IsEqualTo(2);
            await Assert.That(segments[0].Style).IsNotEqualTo(segments[1].Style);
        }
    }

    [Test]
    public async Task Write_CustomObfuscatorMasksSecretSplitAcrossStyledSegments()
    {
        var secretObfuscator = new Mock<ISecretObfuscator>();
        secretObfuscator
            .Setup(x => x.Obfuscate(It.IsAny<string?>(), null))
            .Returns((string? input, object? _) => input!.Replace("abc123", "masked"));

        var output = CaptureFallbackOutput(
            writer => writer.Write(new Markup("[red]abc[/][blue]123[/]")),
            secretObfuscator.Object);

        using (Assert.Multiple())
        {
            await Assert.That(output).Contains("masked");
            await Assert.That(output).DoesNotContain("abc");
            await Assert.That(output).DoesNotContain("123");
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
    public async Task Write_ObfuscatesHyperlinkTargetInPreparedTableTitle()
    {
        var table = new Table().AddColumn("Value").AddRow("content");
        table.Title = new TableTitle("[link=https://host/token]label[/]");
        var renderable = new SecretObfuscatedRenderable(
            table,
            CreateSecretObfuscator("token"));

        var url = renderable.Render(
                RenderOptions.Create(AnsiConsole.Console),
                80)
            .Select(static segment => segment.Link?.Url)
            .First(static value => value is not null)!;

        using (Assert.Multiple())
        {
            await Assert.That(url).Contains("**********");
            await Assert.That(url).DoesNotContain("token");
        }
    }

    [Test]
    public async Task Write_RemovesControlSegmentContainingSecret()
    {
        var renderable = new SecretObfuscatedRenderable(
            new ControlRenderable("secret"),
            CreateSecretObfuscator("secret"));
        var segments = renderable.Render(
                RenderOptions.Create(AnsiConsole.Console),
                80)
            .ToArray();

        await Assert.That(segments).IsEmpty();
    }

    [Test]
    public async Task Write_UsesSafeMaskForSecretsInHyperlinkMetadata()
    {
        var renderable = new SecretObfuscatedRenderable(
            new Markup("[link=https://example.test/token]label[/]"),
            CreateSecretObfuscator(["token", "REDACT"], "[REDACTED]"));
        var url = renderable.Render(
                RenderOptions.Create(AnsiConsole.Console),
                80)
            .Select(static segment => segment.Link?.Url)
            .First(static value => value is not null)!;

        using (Assert.Multiple())
        {
            await Assert.That(url).Contains("[MASKED]");
            await Assert.That(url).DoesNotContain("token");
            await Assert.That(url).DoesNotContain("REDACT");
        }
    }

    [Test]
    public async Task Write_RemovesUnsafeControlMetadataMask()
    {
        var renderable = new SecretObfuscatedRenderable(
            new ControlRenderable("token"),
            CreateSecretObfuscator(["token", "REDACT"], "[REDACTED]"));
        var segments = renderable.Render(
                RenderOptions.Create(AnsiConsole.Console),
                80)
            .ToArray();

        await Assert.That(segments).IsEmpty();
    }

    [Test]
    public async Task Write_RemovesAnsiSequenceContainingSecret()
    {
        var renderable = new SecretObfuscatedRenderable(
            new ControlRenderable("\u001b[31m"),
            CreateSecretObfuscator("31"));

        var segments = renderable.Render(
                RenderOptions.Create(AnsiConsole.Console),
                80)
            .ToArray();

        await Assert.That(segments).IsEmpty();
    }

    [Test]
    public async Task Write_RemovesRelatedControlSequencesTogether()
    {
        var renderable = new SecretObfuscatedRenderable(
            new ControlRenderable("\u001b[?25l", "\u001b[?25h"),
            CreateSecretObfuscator("25h"));

        var segments = renderable.Render(
                RenderOptions.Create(AnsiConsole.Console),
                80)
            .ToArray();

        await Assert.That(segments).IsEmpty();
    }

    [Test]
    public async Task Write_UsesConfiguredMaskWidthWhileObfuscating()
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
    public async Task Write_ReflowsExpandedMaskToRenderWidth()
    {
        var renderable = new SecretObfuscatedRenderable(
            new Text("tiny"),
            CreateSecretObfuscator("tiny"));
        var segments = renderable.Render(
                RenderOptions.Create(AnsiConsole.Console),
                5)
            .ToArray();
        var lines = Segment.SplitLines(segments);

        using (Assert.Multiple())
        {
            await Assert.That(lines).Count().IsEqualTo(2);
            await Assert.That(lines.Max(static line => line.CellCount())).IsLessThanOrEqualTo(5);
            await Assert.That(string.Concat(lines.SelectMany(static line => line)
                    .Select(static segment => segment.Text)))
                .IsEqualTo("**********");
            await Assert.That(renderable.Measure(RenderOptions.Create(AnsiConsole.Console), 5).Max)
                .IsEqualTo(5);
        }
    }

    [Test]
    public async Task Write_PreservesCompositeLayoutWhenMaskWidthDiffers()
    {
        var table = new Table()
            .AddColumn("Value")
            .AddRow("tiny");

        var output = CaptureFallbackOutput(
            writer => writer.Write(table),
            CreateSecretObfuscator("tiny"));
        var lineWidths = output
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(line => new Segment(line).CellCount())
            .Distinct()
            .ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(output).Contains("**********");
            await Assert.That(output).DoesNotContain("tiny");
            await Assert.That(lineWidths).Count().IsEqualTo(1);
        }
    }

    [Test]
    public async Task Write_CustomObfuscatorPreservesCompositeLayout()
    {
        var secretObfuscator = new Mock<ISecretObfuscator>();
        secretObfuscator
            .Setup(x => x.Obfuscate(It.IsAny<string?>(), null))
            .Returns((string? input, object? _) => input!.Replace("tiny", "[REDACTED]"));
        var table = new Table()
            .AddColumn("Value")
            .AddRow("tiny");

        var output = CaptureFallbackOutput(
            writer => writer.Write(table),
            secretObfuscator.Object);
        var lineWidths = output
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(line => new Segment(line).CellCount())
            .Distinct()
            .ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(output).Contains("[REDACTED]");
            await Assert.That(output).DoesNotContain("tiny");
            await Assert.That(lineWidths).Count().IsEqualTo(1);
        }
    }

    [Test]
    public async Task Write_DoesNotReapplyCustomObfuscatorToPreparedComposite()
    {
        var secretObfuscator = new Mock<ISecretObfuscator>();
        secretObfuscator
            .Setup(x => x.Obfuscate(It.IsAny<string?>(), null))
            .Returns("masked");
        var table = new Table()
            .AddColumn("Value")
            .AddRow("tiny");

        var output = CaptureFallbackOutput(
            writer => writer.Write(table),
            secretObfuscator.Object);

        using (Assert.Multiple())
        {
            await Assert.That(output).Contains("masked");
            await Assert.That(output).Contains("┌");
            await Assert.That(output.Split(
                    Environment.NewLine,
                    StringSplitOptions.RemoveEmptyEntries))
                .Count().IsGreaterThan(1);
        }
    }

    [Test]
    public async Task Write_PreservesNestedCompositeLayoutWhenMaskWidthDiffers()
    {
        var grid = new Grid()
            .AddColumn()
            .AddRow("tiny");
        var panel = new Panel(grid);

        var output = CaptureFallbackOutput(
            writer => writer.Write(panel),
            CreateSecretObfuscator("tiny"));
        var lineWidths = output
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Select(line => new Segment(line).CellCount())
            .Distinct()
            .ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(output).Contains("**********");
            await Assert.That(output).DoesNotContain("tiny");
            await Assert.That(lineWidths).Count().IsEqualTo(1);
        }
    }

    [Test]
    public async Task Write_PreparesEveryUnsafeAccessorBackedRenderable()
    {
        var tree = new Tree("secret");
        tree.AddNode("secret");
        IRenderable[] renderables =
        [
            new Align(new Text("secret"), HorizontalAlignment.Left, VerticalAlignment.Top),
            new Columns(new Text("secret")),
            new Padder(new Text("secret"), new Padding(1)),
            new Rows(new Text("secret")),
            tree,
        ];
        var options = RenderOptions.Create(AnsiConsole.Console);
        var obfuscator = CreateSecretObfuscator("secret");

        foreach (var source in renderables)
        {
            var output = string.Concat(new SecretObfuscatedRenderable(source, obfuscator)
                .Render(options, 80)
                .Where(static segment => !segment.IsControlCode)
                .Select(static segment => segment.Text));

            using (Assert.Multiple())
            {
                await Assert.That(output).Contains("**********");
                await Assert.That(output).DoesNotContain("secret");
            }
        }
    }

    [Test]
    public async Task Write_SnapshotsUnhandledRenderablePerWidth()
    {
        var source = new WidthSensitiveRenderable();
        var renderable = new SecretObfuscatedRenderable(
            source,
            CreateMockSecretObfuscator());
        var options = RenderOptions.Create(AnsiConsole.Console);

        _ = renderable.Measure(options, 20);
        var output = string.Concat(renderable.Render(options, 5)
            .Select(static segment => segment.Text));

        using (Assert.Multiple())
        {
            await Assert.That(output).IsEqualTo("5");
            await Assert.That(source.RenderWidths).IsEquivalentTo([20, 5]);
        }
    }

    [Test]
    public async Task Write_PreparesLayoutChildrenBeforeSizingRegions()
    {
        var layout = new Layout("root")
            .SplitColumns(
                new Layout("left", new Text("tiny")).Size(10),
                new Layout("right", new Text("next")).Size(10));
        var renderable = new SecretObfuscatedRenderable(
            layout,
            CreateSecretObfuscator("tiny"));

        var lines = Segment.SplitLines(renderable.Render(
                RenderOptions.Create(AnsiConsole.Console),
                20))
            .Select(static line => string.Concat(line.Select(segment => segment.Text)))
            .ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(lines).Count().IsEqualTo(24);
            await Assert.That(lines[0]).IsEqualTo("**********next      ");
            await Assert.That(lines.All(line => new Segment(line).CellCount() == 20))
                .IsTrue();
        }
    }

    [Test]
    public async Task Write_PreservesEmptyLayoutRegions()
    {
        var layout = new Layout("root")
            .SplitColumns(
                new Layout("left", new Text("tiny")).Size(10),
                new Layout("secret").Size(10));
        var renderable = new SecretObfuscatedRenderable(
            layout,
            CreateSecretObfuscator(["tiny", "secret"], maskValue: null));

        var lines = Segment.SplitLines(renderable.Render(
                RenderOptions.Create(AnsiConsole.Console),
                20))
            .Select(static line => string.Concat(line.Select(segment => segment.Text)))
            .ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(lines).Count().IsEqualTo(24);
            await Assert.That(lines[0]).StartsWith("**********");
            await Assert.That(string.Concat(lines)).DoesNotContain("secret");
            await Assert.That(lines.All(line => new Segment(line).CellCount() == 20))
                .IsTrue();
        }
    }

    [Test]
    public async Task Write_MasksFigletTextBeforeRendering()
    {
        var options = RenderOptions.Create(AnsiConsole.Console);
        var renderable = new SecretObfuscatedRenderable(
            new FigletText("secret"),
            CreateSecretObfuscator("secret"));

        var actual = string.Concat(renderable.Render(options, 120)
            .Select(static segment => segment.Text));
        var expected = string.Concat(((IRenderable) new FigletText("**********")).Render(options, 120)
            .Select(static segment => segment.Text));
        var unmasked = string.Concat(((IRenderable) new FigletText("secret")).Render(options, 120)
            .Select(static segment => segment.Text));

        using (Assert.Multiple())
        {
            await Assert.That(actual).IsEqualTo(expected);
            await Assert.That(actual).IsNotEqualTo(unmasked);
        }
    }

    [Test]
    public async Task Write_PreparesBarChartLabelsBeforeLayout()
    {
        var options = RenderOptions.Create(AnsiConsole.Console);
        var chart = new BarChart
        {
            ShowValues = false,
            Width = 24,
        }.AddItem("tiny", 100, Color.Red);
        var renderable = new SecretObfuscatedRenderable(
            chart,
            CreateSecretObfuscator("tiny"));
        var expectedChart = new BarChart
        {
            ShowValues = false,
            Width = 24,
        }.AddItem("**********", 100, Color.Red);

        var actual = string.Concat(renderable.Render(options, 24)
            .Select(static segment => segment.Text));
        var expected = string.Concat(((IRenderable) expectedChart).Render(options, 24)
            .Select(static segment => segment.Text));

        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    public async Task Write_PreparesBreakdownChartContentBeforeLayout()
    {
        var options = RenderOptions.Create(AnsiConsole.Console);
        var chart = new BreakdownChart
        {
            ValueFormatter = static (_, _) => "tiny",
            Width = 24,
        }.AddItem("tiny", 100, Color.Red);
        var renderable = new SecretObfuscatedRenderable(
            chart,
            CreateSecretObfuscator("tiny"));
        var expectedChart = new BreakdownChart
        {
            ValueFormatter = static (_, _) => "**********",
            Width = 24,
        }.AddItem("**********", 100, Color.Red);

        var actual = string.Concat(renderable.Render(options, 24)
            .Select(static segment => segment.Text));
        var expected = string.Concat(((IRenderable) expectedChart).Render(options, 24)
            .Select(static segment => segment.Text));

        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    public async Task Write_SnapshotsChartValueFormatterResults()
    {
        var formattedValue = "write-time";
        var formatterCalls = 0;
        var chart = new BreakdownChart
        {
            ValueFormatter = (_, _) =>
            {
                formatterCalls++;
                return formattedValue;
            },
            Width = 24,
        }.AddItem("item", 100, Color.Red);
        var renderable = new SecretObfuscatedRenderable(
            chart,
            CreateSecretObfuscator("unrelated"));
        var options = RenderOptions.Create(AnsiConsole.Console);

        var writeTimeOutput = string.Concat(renderable.Render(options, 24)
            .Select(static segment => segment.Text));
        formattedValue = "flush-time";
        var flushOutput = string.Concat(renderable.Render(options, 24)
            .Select(static segment => segment.Text));

        using (Assert.Multiple())
        {
            await Assert.That(writeTimeOutput).Contains("write-time");
            await Assert.That(flushOutput).Contains("write-time");
            await Assert.That(flushOutput).DoesNotContain("flush-time");
            await Assert.That(formatterCalls).IsEqualTo(1);
        }
    }

    [Test]
    public async Task ChartValueFormatterSnapshotDistinguishesSignedZeroAndCulture()
    {
        var formatterCalls = 0;
        var formatter = SecretObfuscatedRenderable.SnapshotValueFormatter((value, culture) =>
        {
            formatterCalls++;
            return $"{BitConverter.DoubleToInt64Bits(value)}:{culture.Name}";
        });
        var invariantCulture = CultureInfo.InvariantCulture;
        var frenchCulture = CultureInfo.GetCultureInfo("fr-FR");

        var positiveZero = formatter(+0.0, invariantCulture);
        var negativeZero = formatter(-0.0, invariantCulture);
        var frenchPositiveZero = formatter(+0.0, frenchCulture);
        var cachedPositiveZero = formatter(+0.0, invariantCulture);

        using (Assert.Multiple())
        {
            await Assert.That(positiveZero).IsNotEqualTo(negativeZero);
            await Assert.That(positiveZero).IsNotEqualTo(frenchPositiveZero);
            await Assert.That(cachedPositiveZero).IsEqualTo(positiveZero);
            await Assert.That(formatterCalls).IsEqualTo(3);
        }
    }

    [Test]
    public async Task Write_EscapesConfiguredMaskInTableTitle()
    {
        var obfuscator = CreateSecretObfuscator("secret", "[REDACTED]");
        var table = new Table()
            .AddColumn("Value")
            .AddRow("value");
        table.Title = new TableTitle("secret");

        var output = CaptureFallbackOutput(writer => writer.Write(table), obfuscator);

        using (Assert.Multiple())
        {
            await Assert.That(output).Contains("[REDACTED]");
            await Assert.That(output).DoesNotContain("secret");
        }
    }

    [Test]
    public async Task Write_PreparesRuleTitleBeforeLayout()
    {
        var obfuscator = CreateSecretObfuscator("tiny", "[REDACTED]");
        var renderable = new SecretObfuscatedRenderable(new Rule("tiny"), obfuscator);
        var segments = renderable.Render(
                RenderOptions.Create(AnsiConsole.Console),
                24)
            .ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(string.Concat(segments.Select(static segment => segment.Text)))
                .Contains("[REDACTED]");
            await Assert.That(Segment.SplitLines(segments).Max(static line => line.CellCount()))
                .IsEqualTo(24);
        }
    }

    [Test]
    public async Task Write_PreparesRuleTitleWithSecretSplitAcrossTags()
    {
        var obfuscator = CreateSecretObfuscator("abc123", "[REDACTED]");
        var renderable = new SecretObfuscatedRenderable(
            new Rule("[red]abc[/][blue]123[/]"),
            obfuscator);
        var segments = renderable.Render(
                RenderOptions.Create(AnsiConsole.Console),
                24)
            .ToArray();
        var renderedText = string.Concat(segments.Select(static segment => segment.Text));

        using (Assert.Multiple())
        {
            await Assert.That(renderedText).Contains("[REDACTED]");
            await Assert.That(renderedText).DoesNotContain("abc");
            await Assert.That(renderedText).DoesNotContain("123");
            await Assert.That(Segment.SplitLines(segments).Max(static line => line.CellCount()))
                .IsEqualTo(24);
        }
    }

    [Test]
    public async Task Write_PreparesEscapedBracketTextBeforeRuleLayout()
    {
        var renderable = new SecretObfuscatedRenderable(
            new Rule("[[tiny]]"),
            CreateSecretObfuscator("tiny", "[REDACTED]"));
        var segments = renderable.Render(
                RenderOptions.Create(AnsiConsole.Console),
                24)
            .ToArray();
        var renderedText = string.Concat(segments.Select(static segment => segment.Text));

        using (Assert.Multiple())
        {
            await Assert.That(renderedText).Contains("[REDACTED]");
            await Assert.That(renderedText).DoesNotContain("tiny");
            await Assert.That(Segment.SplitLines(segments).Max(static line => line.CellCount()))
                .IsEqualTo(24);
        }
    }

    [Test]
    public async Task Write_MeasurePreservesFlexibleMinimumWidth()
    {
        var renderable = new SecretObfuscatedRenderable(
            new Text("1234567890 abcdefghij"),
            CreateSecretObfuscator("unrelated"));

        var measurement = renderable.Measure(
            RenderOptions.Create(AnsiConsole.Console),
            80);

        using (Assert.Multiple())
        {
            await Assert.That(measurement.Min).IsEqualTo(10);
            await Assert.That(measurement.Max).IsEqualTo(21);
        }
    }

    [Test]
    public async Task Write_MeasureDerivesMinimumFromLongerMask()
    {
        var renderable = new SecretObfuscatedRenderable(
            new Text("tiny abcdefghij"),
            CreateSecretObfuscator("tiny", "[REDACTED]"));

        var measurement = renderable.Measure(
            RenderOptions.Create(AnsiConsole.Console),
            80);

        using (Assert.Multiple())
        {
            await Assert.That(measurement.Min).IsEqualTo(10);
            await Assert.That(measurement.Max).IsEqualTo(21);
        }
    }

    [Test]
    public async Task Write_MeasureDerivesMinimumFromShorterMask()
    {
        var renderable = new SecretObfuscatedRenderable(
            new Text("1234567890 abcdefghij"),
            CreateSecretObfuscator("1234567890", "x"));

        var measurement = renderable.Measure(
            RenderOptions.Create(AnsiConsole.Console),
            80);

        using (Assert.Multiple())
        {
            await Assert.That(measurement.Min).IsEqualTo(10);
            await Assert.That(measurement.Max).IsEqualTo(12);
        }
    }

    [Test]
    public async Task Write_AutoSizedTableAccountsForColumnContentWithTitle()
    {
        var table = new Table()
            .AddColumn("Value")
            .AddRow("substantially wider column content");
        table.Title = new TableTitle("Short");
        var renderable = new SecretObfuscatedRenderable(
            table,
            CreateSecretObfuscator("unrelated"));

        var measurement = renderable.Measure(
            RenderOptions.Create(AnsiConsole.Console),
            80);

        using (Assert.Multiple())
        {
            await Assert.That(measurement.Min).IsLessThan(measurement.Max);
            await Assert.That(measurement.Max).IsGreaterThan(20);
        }
    }

    [Test]
    public async Task Write_DoesNotAppendLineBreak()
    {
        var output = CaptureFallbackOutput(writer =>
        {
            writer.Write(new Text("prefix"));
            writer.Write(new Text("suffix"));
        });

        await Assert.That(output).IsEqualTo("prefixsuffix");
    }

    [Test]
    public async Task Write_DoesNotPreserveMaskContainingSecret()
    {
        var secretProvider = new Mock<ISecretProvider>();
        secretProvider.SetupGet(x => x.Version).Returns(0);
        secretProvider.Setup(x => x.GetSnapshot())
            .Returns(new SecretSnapshot(0, ["MASK"]));
        var obfuscator = new SecretObfuscator(
            secretProvider.Object,
            Microsoft.Extensions.Options.Options.Create(new SecretMaskingOptions
            {
                MaskValue = "[MASK]",
            }));
        var output = CaptureFallbackOutput(
            writer => writer.Write(new Text("MASK")),
            obfuscator);

        using (Assert.Multiple())
        {
            await Assert.That(output).IsNotEmpty();
            await Assert.That(output).DoesNotContain("MASK");
        }
    }

    [Test]
    public async Task WriteMarkupLine_ObfuscatesSplitSecretInModuleBuffer()
    {
        var output = await RunAsync<WriteSplitSecretModule>();

        await Assert.That(output).Contains("******");
        await Assert.That(output).DoesNotContain("abc");
        await Assert.That(output).DoesNotContain("123");
    }

    [Test]
    public async Task LifecycleHooks_UseModuleConsoleWriter()
    {
        var output = await RunAsync<ReadyOutputModule>(
            builder => builder.AddModuleEventHandler<WriteLifecycleOutputHandler>());

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
        AnsiSupport ansiSupport = AnsiSupport.No,
        ISecretProvider? secretProvider = null)
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
            secretProvider ??= new Mock<ISecretProvider>().Object;

            write(new ConsoleWriter(secretObfuscator, secretProvider, AnsiConsole.Console));
            return output.ToString();
        }
        finally
        {
            AnsiConsole.Console = originalConsole;
        }
    }

    private static IAnsiConsole CreateConsole(TextWriter output) =>
        AnsiConsole.Create(new AnsiConsoleSettings
        {
            Out = new AnsiConsoleOutput(output),
            Ansi = AnsiSupport.No,
            ColorSystem = ColorSystemSupport.NoColors,
        });

    private static ISecretObfuscator CreateMockSecretObfuscator()
    {
        var secretObfuscator = new Mock<ISecretObfuscator>();
        secretObfuscator
            .Setup(x => x.Obfuscate(It.IsAny<string?>(), null))
            .Returns((string? input, object? _) => input!.Replace("secret", "********"));
        return secretObfuscator.Object;
    }

    private static ISecretObfuscator CreateSecretObfuscator(params string[] secrets)
        => CreateSecretObfuscator(secrets, maskValue: null);

    private static ISecretObfuscator CreateSecretObfuscator(string secret, string maskValue)
        => CreateSecretObfuscator([secret], maskValue);

    private static ISecretObfuscator CreateSecretObfuscator(
        string[] secrets,
        string? maskValue)
    {
        var secretProvider = new Mock<ISecretProvider>();
        secretProvider.SetupGet(x => x.Version).Returns(0);
        secretProvider.Setup(x => x.GetSnapshot()).Returns(new SecretSnapshot(0, secrets));
        return new SecretObfuscator(
            secretProvider.Object,
            Microsoft.Extensions.Options.Options.Create(new SecretMaskingOptions
            {
                MaskValue = maskValue ?? "**********",
            }));
    }

    private static async Task AssertFallbackOutputIsObfuscated(string output)
    {
        await Assert.That(output).Contains("********");
        await Assert.That(output).DoesNotContain("secret");
    }
}
