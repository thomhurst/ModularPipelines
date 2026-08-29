using ModularPipelines.Secrets;
using MEL.Spectre;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using ModularPipelines.Console;
using ModularPipelines.Engine;
using ModularPipelines.Engine.BuildSystemFormatters;
using ModularPipelines.Enums;
using ModularPipelines.TestHelpers;
using Moq;

namespace ModularPipelines.UnitTests.Console;

public class ModuleOutputBufferTests
{
    [Test]
    public async Task OutputExcerptSurvivesConsoleFlush()
    {
        var writer = new StringWriter();
        var loggerControl = new SynchronousLoggerControl(writer);
        var buffer = new ModuleOutputBuffer(
            typeof(ModuleOutputBufferTests),
            outputExcerptMaximumBytes: 1024);
        buffer.WriteLine("retained output");

        await buffer.FlushToAsync(
            writer,
            new DefaultFormatter(),
            loggerControl,
            loggerControl,
            OutputFlushKind.Complete);

        await Assert.That(buffer.GetOutputExcerpt()!.StdoutTail).Contains("retained output");
    }

    [Test]
    public async Task OutputExcerptIsDisabledByDefault()
    {
        var buffer = new ModuleOutputBuffer(typeof(ModuleOutputBufferTests));

        buffer.WriteLine("ordinary output");

        await Assert.That(buffer.GetOutputExcerpt()).IsNull();
    }

    [Test]
    public async Task Flush_Writes_Group_Commands_Without_Markup_Rendering()
    {
        var writer = new StringWriter();
        var loggerControl = new SynchronousLoggerControl(writer);
        var buffer = new ModuleOutputBuffer(typeof(ModuleOutputBufferTests));
        buffer.WriteLine("output");

        await buffer.FlushToAsync(
            writer,
            new MarkupLikeBuildSystemFormatter(),
            loggerControl,
            loggerControl,
            OutputFlushKind.Complete);

        await Assert.That(writer.ToString()).Contains("[red]::group::[/]");
    }

    [Test]
    public async Task Flush_Preserves_Buffered_Raw_Group_Command_Order()
    {
        const string startCommand = "[red]::group::dynamic[name][/]";
        const string body = "section body";
        const string endCommand = "::endgroup::dynamic";
        var writer = new StringWriter();
        var loggerControl = new SynchronousLoggerControl(writer);
        var formatter = new MarkupLikeBuildSystemFormatter();
        var buffer = new ModuleOutputBuffer(typeof(ModuleOutputBufferTests));

        buffer.WriteGroupCommand(formatter, startCommand);
        buffer.WriteLine(body);
        buffer.WriteGroupCommand(formatter, endCommand);

        await buffer.FlushToAsync(
            writer,
            formatter,
            loggerControl,
            loggerControl,
            OutputFlushKind.Complete);

        var output = writer.ToString();
        var startIndex = output.IndexOf(startCommand, StringComparison.Ordinal);
        var bodyIndex = output.IndexOf(body, StringComparison.Ordinal);
        var endIndex = output.IndexOf(endCommand, StringComparison.Ordinal);

        await Assert.That(startIndex).IsGreaterThanOrEqualTo(0);
        await Assert.That(bodyIndex).IsGreaterThan(startIndex);
        await Assert.That(endIndex).IsGreaterThan(bodyIndex);
    }

    [Test]
    public async Task Flush_Renders_Local_Group_Header_Markup()
    {
        var writer = new StringWriter();
        var loggerControl = new SynchronousLoggerControl(writer);
        var buffer = new ModuleOutputBuffer(typeof(ModuleOutputBufferTests));
        buffer.WriteLine("output");

        await buffer.FlushToAsync(
            writer,
            new DefaultFormatter(),
            loggerControl,
            loggerControl,
            OutputFlushKind.Complete);

        var output = writer.ToString();
        await Assert.That(output).Contains("▶");
        await Assert.That(output).DoesNotContain("[bold cyan]");
    }

    [Test]
    public async Task Flush_Writes_Structured_Logs_Before_Group_End()
    {
        var writer = new StringWriter();
        var loggerControl = new SynchronousLoggerControl(writer);
        var buffer = CreateBufferWithStructuredLog();

        await buffer.FlushToAsync(
            writer,
            new GitHubActionsFormatter(),
            loggerControl,
            loggerControl,
            OutputFlushKind.Complete);

        var output = writer.ToString();
        var structuredLog = output.IndexOf("structured log", StringComparison.Ordinal);
        var groupEnd = output.IndexOf("::endgroup::", StringComparison.Ordinal);

        await Assert.That(structuredLog).IsGreaterThanOrEqualTo(0);
        await Assert.That(groupEnd).IsGreaterThan(structuredLog);
    }

    [Test]
    public async Task Flush_Preserves_Structured_And_Direct_Output_Order()
    {
        var writer = new StringWriter();
        var loggerControl = new SynchronousLoggerControl(writer);
        var buffer = CreateBufferWithStructuredLog();
        buffer.WriteLine("direct output");

        await buffer.FlushToAsync(
            writer,
            new GitHubActionsFormatter(),
            loggerControl,
            loggerControl,
            OutputFlushKind.Complete);

        var output = writer.ToString();
        var groupStart = output.IndexOf("::group::", StringComparison.Ordinal);
        var structuredLog = output.IndexOf("structured log", StringComparison.Ordinal);
        var directOutput = output.IndexOf("direct output", StringComparison.Ordinal);
        var groupEnd = output.IndexOf("::endgroup::", StringComparison.Ordinal);

        await Assert.That(groupStart).IsGreaterThanOrEqualTo(0);
        await Assert.That(structuredLog).IsGreaterThan(groupStart);
        await Assert.That(directOutput).IsGreaterThan(structuredLog);
        await Assert.That(groupEnd).IsGreaterThan(directOutput);
    }

    [Test]
    public async Task Flush_Renders_Markup_For_Direct_Output()
    {
        var writer = new StringWriter();
        var loggerControl = new SynchronousLoggerControl(writer);
        var buffer = new ModuleOutputBuffer(typeof(ModuleOutputBufferTests));
        buffer.WriteLine("[green]direct output[/]");

        await buffer.FlushToAsync(
            writer,
            new GitHubActionsFormatter(),
            loggerControl,
            loggerControl,
            OutputFlushKind.Complete);

        var output = writer.ToString();
        await Assert.That(output).Contains("direct output");
        await Assert.That(output).DoesNotContain("[green]");
    }

    [Test]
    public async Task DirectConsole_IsCachedPerWriter()
    {
        var buffer = new ModuleOutputBuffer(typeof(ModuleOutputBufferTests));
        using var firstWriter = new StringWriter();
        using var secondWriter = new StringWriter();

        var firstConsole = buffer.GetDirectConsole(firstWriter);

        await Assert.That(buffer.GetDirectConsole(firstWriter)).IsSameReferenceAs(firstConsole);
        await Assert.That(buffer.GetDirectConsole(secondWriter)).IsNotSameReferenceAs(firstConsole);
    }

    [Test]
    public async Task LogEventAddedAfterCompletion_IsIgnored()
    {
        var buffer = new ModuleOutputBuffer(typeof(ModuleOutputBufferTests));

        buffer.MarkComplete();
        buffer.AddLogEvent(new BufferedLogEvent<string>(
            LogLevel.Information,
            default,
            "late log",
            "late log",
            null,
            static (state, _) => state,
            new PassthroughSecretObfuscator()));

        await Assert.That(buffer.HasOutput).IsFalse();
        await Assert.That(buffer.NeedsCompletionFlush).IsFalse();
    }

    [Test]
    public async Task BufferedLogEvent_PreservesGenericTypeForNullState()
    {
        var logger = new RecordingLogger();
        var logEvent = new BufferedLogEvent<string?>(
            LogLevel.Information,
            default,
            null,
            null,
            null,
            static (state, _) => state ?? "null-state",
            new PassthroughSecretObfuscator());

        logEvent.WriteTo(logger);

        await Assert.That(logger.StateTypes).HasSingleItem().And.IsEquivalentTo([typeof(string)]);
    }

    [Test]
    public async Task BufferedLogEvent_DoesNotEnumerateArbitraryStructuredState()
    {
        var state = new ThrowingEnumerableLogState();

        var logEvent = new BufferedLogEvent<ThrowingEnumerableLogState>(
            LogLevel.Information,
            default,
            state,
            state,
            null,
            static (_, _) => "safe message",
            new PassthroughSecretObfuscator());

        await Assert.That(logEvent.Stream).IsEqualTo(ModuleOutputStream.StandardOutput);
    }

    [Test]
    public async Task BufferedLogEvent_RecognizesIndexedCommandErrorState()
    {
        KeyValuePair<string, object?>[] state = [new("CommandError", true)];

        var logEvent = new BufferedLogEvent<KeyValuePair<string, object?>[]>(
            LogLevel.Error,
            default,
            state,
            state,
            null,
            static (_, _) => "command error",
            new PassthroughSecretObfuscator());

        await Assert.That(logEvent.Stream).IsEqualTo(ModuleOutputStream.StandardError);
    }

    [Test]
    public async Task BufferedLogEvent_FormatsOnceAndObfuscatesEveryTime()
    {
        var formatterCalls = 0;
        var secretObfuscator = new Mock<ISecretObfuscator>();
        secretObfuscator.SetupGet(x => x.HasSecrets).Returns(true);
        secretObfuscator
            .Setup(x => x.Obfuscate(It.IsAny<string?>(), null))
            .Returns((string? value, object? _) => value?.Replace("secret", "***") ?? string.Empty);
        var logEvent = new BufferedLogEvent<string>(
            LogLevel.Information,
            default,
            "secret",
            "***",
            null,
            (state, _) =>
            {
                formatterCalls++;
                return $"value:{state}";
            },
            secretObfuscator.Object);

        logEvent.WriteTo(new RecordingLogger());
        logEvent.WriteTo(new RecordingLogger());
        _ = logEvent.FormatMessageWithLevel();
        _ = logEvent.FormatMessageWithLevel();

        await Assert.That(formatterCalls).IsEqualTo(1);
        secretObfuscator.Verify(
            x => x.Obfuscate("value:secret", null),
            Times.Exactly(4));
    }

    [Test]
    public async Task BufferedLogEvent_ReobfuscatesMessageWithCurrentSecrets()
    {
        const string secret = "late-registered-secret";
        var redactSecret = false;
        var secretObfuscator = new Mock<ISecretObfuscator>();
        secretObfuscator.SetupGet(x => x.HasSecrets).Returns(() => redactSecret);
        secretObfuscator
            .Setup(x => x.Obfuscate(It.IsAny<string?>(), null))
            .Returns((string? value, object? _) => redactSecret
                ? value?.Replace(secret, "***", StringComparison.Ordinal) ?? string.Empty
                : value ?? string.Empty);
        var logEvent = new BufferedLogEvent<string>(
            LogLevel.Information,
            default,
            secret,
            secret,
            null,
            static (state, _) => state,
            secretObfuscator.Object);

        var beforeRegistration = logEvent.FormatMessageWithLevel();
        redactSecret = true;
        var afterRegistration = logEvent.FormatMessageWithLevel();

        using (Assert.Multiple())
        {
            await Assert.That(beforeRegistration).Contains(secret);
            await Assert.That(afterRegistration).Contains("***");
            await Assert.That(afterRegistration).DoesNotContain(secret);
        }
    }

    [Test]
    public async Task BufferedLogEvent_ReobfuscatesExceptionWithCurrentSecrets()
    {
        const string secret = "late-registered-secret";
        var redactSecret = false;
        var secretObfuscator = new Mock<ISecretObfuscator>();
        secretObfuscator.SetupGet(x => x.HasSecrets).Returns(() => redactSecret);
        secretObfuscator
            .Setup(x => x.Obfuscate(It.IsAny<string?>(), null))
            .Returns((string? value, object? _) => redactSecret
                ? value?.Replace(secret, "***", StringComparison.Ordinal) ?? string.Empty
                : value ?? string.Empty);
        var logEvent = new BufferedLogEvent<string>(
            LogLevel.Error,
            default,
            "failure",
            "failure",
            new InvalidOperationException(secret),
            static (state, _) => state,
            secretObfuscator.Object);

        redactSecret = true;

        var formattedException = logEvent.FormatException();

        await Assert.That(formattedException).Contains("***");
        await Assert.That(formattedException).DoesNotContain(secret);
    }

    [Test]
    public async Task ConsoleOutputAddedAfterCompletion_IsRetained()
    {
        var buffer = new ModuleOutputBuffer(typeof(ModuleOutputBufferTests));

        buffer.MarkComplete();
        buffer.WriteLine("retained output");

        await Assert.That(buffer.HasOutput).IsTrue();
        await Assert.That(buffer.NeedsCompletionFlush).IsTrue();
    }

    [Test]
    public async Task IncrementalFlush_Marks_Module_As_InProgress()
    {
        var writer = new StringWriter();
        var loggerControl = new SynchronousLoggerControl(writer);
        var buffer = new ModuleOutputBuffer(typeof(ModuleOutputBufferTests));
        buffer.WriteLine("partial output");

        await buffer.FlushToAsync(
            writer,
            new GitHubActionsFormatter(),
            loggerControl,
            loggerControl,
            OutputFlushKind.Incremental);

        var output = writer.ToString();
        await Assert.That(output).Contains("ModuleOutputBufferTests …");
        await Assert.That(output).Contains("partial output");
        await Assert.That(output).DoesNotContain("ModuleOutputBufferTests ✓");
        await Assert.That(buffer.HasOutput).IsFalse();
        await Assert.That(buffer.NeedsCompletionFlush).IsFalse();
    }

    [Test]
    public async Task IncrementalFlush_RearmsOutputThreshold()
    {
        var thresholdRequests = 0;
        var writer = new StringWriter();
        var loggerControl = new SynchronousLoggerControl(writer);
        var buffer = new ModuleOutputBuffer(
            typeof(ModuleOutputBufferTests),
            outputFlushThreshold: 2,
            _ => thresholdRequests++);

        buffer.WriteLine("first");
        buffer.WriteLine("second");
        await Assert.That(thresholdRequests).IsEqualTo(1);

        await buffer.FlushToAsync(
            writer,
            new GitHubActionsFormatter(),
            loggerControl,
            loggerControl,
            OutputFlushKind.Incremental);
        buffer.WriteLine("third");
        buffer.WriteLine("fourth");

        await Assert.That(thresholdRequests).IsEqualTo(2);
    }

    [Test]
    public async Task CompleteFlush_RendersSuccessMarkerForModuleOutput()
    {
        var writer = new StringWriter();
        var loggerControl = new SynchronousLoggerControl(writer);
        var buffer = new ModuleOutputBuffer(typeof(ModuleOutputBufferTests));
        buffer.WriteLine("completed output");

        await buffer.FlushToAsync(
            writer,
            new GitHubActionsFormatter(),
            loggerControl,
            loggerControl,
            OutputFlushKind.Complete);

        await Assert.That(writer.ToString()).Contains("ModuleOutputBufferTests ✓");
    }

    [Test]
    [Arguments(ModuleStatus.Skipped, "⊘")]
    [Arguments(ModuleStatus.FailureIgnored, "⚠")]
    [Arguments(ModuleStatus.RestoredFromHistory, "↻")]
    [Arguments(ModuleStatus.RestoredFromCache, "↻")]
    public async Task CompleteFlush_RendersMarkerForFinalModuleStatus(
        ModuleStatus status,
        string expectedMarker)
    {
        var writer = new StringWriter();
        var loggerControl = new SynchronousLoggerControl(writer);
        var buffer = new ModuleOutputBuffer(typeof(ModuleOutputBufferTests));
        buffer.WriteLine("module output");
        buffer.SetStatus(status);

        await buffer.FlushToAsync(
            writer,
            new GitHubActionsFormatter(),
            loggerControl,
            loggerControl,
            OutputFlushKind.Complete);

        var output = writer.ToString();
        await Assert.That(output).Contains($"ModuleOutputBufferTests {expectedMarker}");
        await Assert.That(output).DoesNotContain("ModuleOutputBufferTests ✓");
    }

    [Test]
    public async Task FailedPipelineOutput_DoesNotRenderSuccessMarker()
    {
        var writer = new StringWriter();
        var loggerControl = new SynchronousLoggerControl(writer);
        var buffer = new ModuleOutputBuffer(
            "Pipeline",
            typeof(void),
            showSuccessMarker: false);
        buffer.WriteLine("Pipeline Failed");

        await buffer.FlushToAsync(
            writer,
            new GitHubActionsFormatter(),
            loggerControl,
            loggerControl,
            OutputFlushKind.Complete);

        var output = writer.ToString();
        await Assert.That(output).Contains("Pipeline Failed");
        await Assert.That(output).DoesNotContain("Pipeline ✓");
    }

    [Test]
    public async Task BlankPipelineOutput_DoesNotRenderEmptyGroup()
    {
        var writer = new StringWriter();
        var loggerControl = new SynchronousLoggerControl(writer);
        var buffer = new ModuleOutputBuffer(
            "Pipeline",
            typeof(void),
            showSuccessMarker: false);
        buffer.WriteLine(string.Empty);

        await buffer.FlushToAsync(
            writer,
            new GitHubActionsFormatter(),
            loggerControl,
            loggerControl,
            OutputFlushKind.Complete);

        var output = writer.ToString();
        using (Assert.Multiple())
        {
            await Assert.That(output).DoesNotContain("::group::Pipeline");
            await Assert.That(output).DoesNotContain("::endgroup::");
        }
    }

    [Test]
    public async Task BlankPipelineOutput_DoesNotAcquireRenderGate()
    {
        var writer = new StringWriter();
        var renderGateWaitStarted = new TaskCompletionSource(
            TaskCreationOptions.RunContinuationsAsynchronously);
        var loggerControl = new SynchronousLoggerControl(writer)
        {
            RenderGateWaitStarted = renderGateWaitStarted,
        };
        var buffer = new ModuleOutputBuffer(
            "Pipeline",
            typeof(void),
            showSuccessMarker: false);
        buffer.WriteLine(string.Empty);

        await buffer.FlushToAsync(
            writer,
            new GitHubActionsFormatter(),
            loggerControl,
            loggerControl,
            OutputFlushKind.Complete);

        await Assert.That(renderGateWaitStarted.Task.IsCompleted).IsFalse();
    }

    [Test]
    public async Task IncrementalFlush_DoesNotDrainCompletedModule()
    {
        var writer = new StringWriter();
        var loggerControl = new SynchronousLoggerControl(writer);
        var buffer = new ModuleOutputBuffer(typeof(ModuleOutputBufferTests));
        buffer.WriteLine("completed output");
        buffer.MarkComplete();

        await buffer.FlushToAsync(
            writer,
            new GitHubActionsFormatter(),
            loggerControl,
            loggerControl,
            OutputFlushKind.Incremental);

        await Assert.That(writer.ToString()).IsEmpty();
        await Assert.That(buffer.HasOutput).IsTrue();

        await buffer.FlushToAsync(
            writer,
            new GitHubActionsFormatter(),
            loggerControl,
            loggerControl,
            OutputFlushKind.Complete);
        await Assert.That(writer.ToString()).Contains("completed output");
    }

    [Test]
    public async Task CompleteFlush_SuppressesEmptyGroupAfterIncrementalDrain()
    {
        var writer = new StringWriter();
        var loggerControl = new SynchronousLoggerControl(writer);
        var buffer = new ModuleOutputBuffer(typeof(ModuleOutputBufferTests));
        buffer.WriteLine("partial output");

        await buffer.FlushToAsync(
            writer,
            new GitHubActionsFormatter(),
            loggerControl,
            loggerControl,
            OutputFlushKind.Incremental);
        buffer.MarkComplete();
        await buffer.FlushToAsync(
            writer,
            new GitHubActionsFormatter(),
            loggerControl,
            loggerControl,
            OutputFlushKind.Complete);

        var output = writer.ToString();
        await Assert.That(output).Contains("ModuleOutputBufferTests …");
        await Assert.That(output).DoesNotContain("ModuleOutputBufferTests ✓");
        await Assert.That(output).Contains("partial output");
        await Assert.That(buffer.NeedsCompletionFlush).IsFalse();
    }

    [Test]
    public async Task CompleteFlush_RendersFailureHeaderAfterIncrementalDrain()
    {
        var writer = new StringWriter();
        var loggerControl = new SynchronousLoggerControl(writer);
        var buffer = new ModuleOutputBuffer(typeof(ModuleOutputBufferTests));
        buffer.WriteLine("partial output");

        await buffer.FlushToAsync(
            writer,
            new GitHubActionsFormatter(),
            loggerControl,
            loggerControl,
            OutputFlushKind.Incremental);
        buffer.WriteLine("failure output");
        await buffer.FlushToAsync(
            writer,
            new GitHubActionsFormatter(),
            loggerControl,
            loggerControl,
            OutputFlushKind.Incremental);
        buffer.SetException(new InvalidOperationException("module failure"));
        buffer.MarkComplete();

        await Assert.That(buffer.NeedsCompletionFlush).IsTrue();
        await buffer.FlushToAsync(
            writer,
            new GitHubActionsFormatter(),
            loggerControl,
            loggerControl,
            OutputFlushKind.Complete);

        var output = writer.ToString();
        await Assert.That(output).Contains("ModuleOutputBufferTests ✗ (continued)");
        await Assert.That(output).Contains(nameof(InvalidOperationException));
        await Assert.That(buffer.NeedsCompletionFlush).IsFalse();
    }

    [Test]
    public async Task CompleteFlush_SuppressesFailureGroupWithoutOutput()
    {
        var writer = new StringWriter();
        var loggerControl = new SynchronousLoggerControl(writer);
        var buffer = new ModuleOutputBuffer(typeof(ModuleOutputBufferTests));
        buffer.SetException(new InvalidOperationException("module failure"));
        buffer.MarkComplete();

        await Assert.That(buffer.NeedsCompletionFlush).IsFalse();
        await buffer.FlushToAsync(
            writer,
            new GitHubActionsFormatter(),
            loggerControl,
            loggerControl,
            OutputFlushKind.Complete);

        await Assert.That(writer.ToString()).IsEmpty();
    }

    [Test]
    public async Task CompleteFlush_RendersSilentFailureWhenResultsAreHidden()
    {
        var writer = new StringWriter();
        var loggerControl = new SynchronousLoggerControl(writer);
        var buffer = new ModuleOutputBuffer(
            nameof(ModuleOutputBufferTests),
            typeof(ModuleOutputBufferTests),
            showFailureHeaderWithoutOutput: true);
        buffer.SetException(new InvalidOperationException("module failure"));
        buffer.MarkComplete();

        await Assert.That(buffer.NeedsCompletionFlush).IsTrue();
        await buffer.FlushToAsync(
            writer,
            new GitHubActionsFormatter(),
            loggerControl,
            loggerControl,
            OutputFlushKind.Complete);

        var output = writer.ToString();
        await Assert.That(output).Contains("ModuleOutputBufferTests ✗");
        await Assert.That(output).Contains(nameof(InvalidOperationException));
    }

    [Test]
    public async Task LaterFlushes_AreLabelledAsContinued()
    {
        var writer = new StringWriter();
        var loggerControl = new SynchronousLoggerControl(writer);
        var buffer = new ModuleOutputBuffer(typeof(ModuleOutputBufferTests));
        buffer.WriteLine("first output");

        await buffer.FlushToAsync(
            writer,
            new GitHubActionsFormatter(),
            loggerControl,
            loggerControl,
            OutputFlushKind.Incremental);
        buffer.WriteLine("second output");
        buffer.MarkComplete();
        await buffer.FlushToAsync(
            writer,
            new GitHubActionsFormatter(),
            loggerControl,
            loggerControl,
            OutputFlushKind.Complete);

        var output = writer.ToString();
        await Assert.That(output).Contains("ModuleOutputBufferTests … (");
        await Assert.That(output).Contains("ModuleOutputBufferTests ✓ (continued) (");
        await Assert.That(output).Contains("first output");
        await Assert.That(output).Contains("second output");
    }

    [Test]
    public async Task LaterIncrementalFlushes_AreLabelledAsContinued()
    {
        var writer = new StringWriter();
        var loggerControl = new SynchronousLoggerControl(writer);
        var buffer = new ModuleOutputBuffer(typeof(ModuleOutputBufferTests));
        buffer.WriteLine("first output");

        await buffer.FlushToAsync(
            writer,
            new GitHubActionsFormatter(),
            loggerControl,
            loggerControl,
            OutputFlushKind.Incremental);
        buffer.WriteLine("second output");
        await buffer.FlushToAsync(
            writer,
            new GitHubActionsFormatter(),
            loggerControl,
            loggerControl,
            OutputFlushKind.Incremental);

        await Assert.That(writer.ToString()).Contains("ModuleOutputBufferTests … (continued) (");
    }

    [Test]
    public async Task VisibleOutputAfterFilteredIncrementalFlush_IsNotLabelledAsContinued()
    {
        var writer = new StringWriter();
        var loggerControl = new SynchronousLoggerControl(writer);
        var buffer = CreateBufferWithStructuredLog(isSpectreEnabled: static _ => false);

        await buffer.FlushToAsync(
            writer,
            new GitHubActionsFormatter(),
            new RecordingLogger(),
            loggerControl,
            OutputFlushKind.Incremental);
        buffer.WriteLine("visible output");
        await buffer.FlushToAsync(
            writer,
            new GitHubActionsFormatter(),
            loggerControl,
            loggerControl,
            OutputFlushKind.Incremental);

        var output = writer.ToString();
        await Assert.That(output).Contains("ModuleOutputBufferTests … (");
        await Assert.That(output).DoesNotContain("(continued)");
        await Assert.That(output).Contains("visible output");
    }

    [Test]
    public async Task CompletionNeed_Remains_While_IncrementalFlush_Owns_Output()
    {
        var writer = new StringWriter();
        var loggerControl = new SynchronousLoggerControl(writer);
        var buffer = CreateBufferWithStructuredLog();
        var renderingStarted = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var releaseRendering = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        loggerControl.AfterLog = () =>
        {
            renderingStarted.TrySetResult();
            releaseRendering.Task.GetAwaiter().GetResult();
        };

        var incrementalFlush = Task.Run(() => buffer.FlushToAsync(
            writer,
            new GitHubActionsFormatter(),
            loggerControl,
            loggerControl,
            OutputFlushKind.Incremental));
        await renderingStarted.Task.WaitAsync(TimeSpan.FromSeconds(1));

        try
        {
            await Assert.That(buffer.HasOutput).IsFalse();
            await Assert.That(buffer.NeedsCompletionFlush).IsTrue();
            buffer.MarkComplete();
        }
        finally
        {
            releaseRendering.TrySetResult();
        }

        await incrementalFlush;
        await buffer.FlushToAsync(
            writer,
            new GitHubActionsFormatter(),
            loggerControl,
            loggerControl,
            OutputFlushKind.Complete);

        await Assert.That(writer.ToString()).DoesNotContain("ModuleOutputBufferTests ✓");
        await Assert.That(buffer.NeedsCompletionFlush).IsFalse();
    }

    [Test]
    public async Task Flush_Keeps_Concurrent_Logs_Outside_Module_Group()
    {
        var writer = new StringWriter();
        var loggerControl = new SynchronousLoggerControl(writer);
        var buffer = CreateBufferWithStructuredLog();
        Task? outsideLog = null;
        using var gateAttemptCompleted = new ManualResetEventSlim();
        loggerControl.AfterLog = () =>
        {
            outsideLog = Task.Run(() =>
            {
                if (!loggerControl.TryWrite("outside log", TimeSpan.FromMilliseconds(100)))
                {
                    gateAttemptCompleted.Set();
                    loggerControl.Write("outside log");
                }
                else
                {
                    gateAttemptCompleted.Set();
                }
            });
            gateAttemptCompleted.Wait();
        };

        await buffer.FlushToAsync(
            writer,
            new GitHubActionsFormatter(),
            loggerControl,
            loggerControl,
            OutputFlushKind.Complete);
        await outsideLog!;

        var output = writer.ToString();
        var groupEnd = output.IndexOf("::endgroup::", StringComparison.Ordinal);
        var outside = output.IndexOf("outside log", StringComparison.Ordinal);

        await Assert.That(groupEnd).IsGreaterThanOrEqualTo(0);
        await Assert.That(outside).IsGreaterThan(groupEnd);
    }

    [Test]
    public async Task Flush_Observes_Cancellation()
    {
        var writer = new StringWriter();
        var loggerControl = new SynchronousLoggerControl(writer);
        var buffer = CreateBufferWithStructuredLog();
        using var cancellationTokenSource = new CancellationTokenSource();
        await cancellationTokenSource.CancelAsync();

        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await buffer.FlushToAsync(
                writer,
                new GitHubActionsFormatter(),
                loggerControl,
                loggerControl,
                OutputFlushKind.Complete,
                cancellationToken: cancellationTokenSource.Token));

        await Assert.That(buffer.HasOutput).IsTrue();
    }

    [Test]
    public async Task Flush_Cancellation_Closes_Group_And_Retains_Unrendered_Output()
    {
        var writer = new StringWriter();
        var loggerControl = new SynchronousLoggerControl(writer);
        var buffer = CreateBufferWithStructuredLog();
        buffer.WriteLine("remaining output");
        using var cancellationTokenSource = new CancellationTokenSource();
        loggerControl.AfterLog = cancellationTokenSource.Cancel;

        await Assert.ThrowsAsync<OperationCanceledException>(async () =>
            await buffer.FlushToAsync(
                writer,
                new GitHubActionsFormatter(),
                loggerControl,
                loggerControl,
                OutputFlushKind.Complete,
                cancellationToken: cancellationTokenSource.Token));

        var cancelledOutput = writer.ToString();
        await Assert.That(cancelledOutput.IndexOf("::endgroup::", StringComparison.Ordinal))
            .IsGreaterThan(cancelledOutput.IndexOf("structured log", StringComparison.Ordinal));
        await Assert.That(buffer.HasOutput).IsTrue();

        await buffer.FlushToAsync(
            writer,
            new GitHubActionsFormatter(),
            loggerControl,
            loggerControl,
            OutputFlushKind.Complete);

        await Assert.That(writer.ToString()).Contains("remaining output");
        await Assert.That(buffer.HasOutput).IsFalse();
    }

    [Test]
    public async Task Flush_RenderingFailure_Retains_Failed_Output()
    {
        var writer = new StringWriter();
        var loggerControl = new SynchronousLoggerControl(writer)
        {
            LogException = new InvalidOperationException("render failed"),
        };
        var buffer = CreateBufferWithStructuredLog();
        buffer.WriteLine("remaining output");

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await buffer.FlushToAsync(
                writer,
                new GitHubActionsFormatter(),
                loggerControl,
                loggerControl,
                OutputFlushKind.Complete));

        await Assert.That(buffer.HasOutput).IsTrue();

        loggerControl.LogException = null;
        await buffer.FlushToAsync(
            writer,
            new GitHubActionsFormatter(),
            loggerControl,
            loggerControl,
            OutputFlushKind.Complete);

        await Assert.That(writer.ToString()).Contains("structured log");
        await Assert.That(writer.ToString()).Contains("remaining output");
        await Assert.That(buffer.HasOutput).IsFalse();
    }

    [Test]
    public async Task IncrementalFlush_Retries_LogAcceptedBeforeProviderFailure()
    {
        var writer = new StringWriter();
        var loggerControl = new SynchronousLoggerControl(writer)
        {
            LogExceptionAfterWrite = new InvalidOperationException("provider failed after delivery"),
        };
        var buffer = CreateBufferWithStructuredLog();
        buffer.WriteLine("remaining output");

        await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await buffer.FlushToAsync(
                writer,
                new GitHubActionsFormatter(),
                loggerControl,
                loggerControl,
                OutputFlushKind.Incremental));

        loggerControl.LogExceptionAfterWrite = null;
        await buffer.FlushToAsync(
            writer,
            new GitHubActionsFormatter(),
            loggerControl,
            loggerControl,
            OutputFlushKind.Incremental);

        await Assert.That(CountOccurrences(writer.ToString(), "structured log")).IsEqualTo(2);
        await Assert.That(writer.ToString()).Contains("remaining output");
        await Assert.That(buffer.HasOutput).IsFalse();
    }

    [Test]
    public async Task Flush_CancellationInterruptsRenderGateWait()
    {
        var writer = new StringWriter();
        var renderGateWaitStarted = new TaskCompletionSource(
            TaskCreationOptions.RunContinuationsAsynchronously);
        var loggerControl = new SynchronousLoggerControl(writer)
        {
            RenderGateWaitStarted = renderGateWaitStarted,
        };
        var buffer = CreateBufferWithStructuredLog();
        var lockAcquired = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var releaseLock = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var lockHolder = Task.Run(() =>
        {
            lock (loggerControl.SynchronizationLock)
            {
                lockAcquired.TrySetResult();
                releaseLock.Task.GetAwaiter().GetResult();
            }
        });

        await lockAcquired.Task.WaitAsync(TestHostSettings.DefaultTestTimeout);
        using var cancellationTokenSource = new CancellationTokenSource();
        try
        {
            var flush = buffer.FlushToAsync(
                writer,
                new GitHubActionsFormatter(),
                loggerControl,
                loggerControl,
                OutputFlushKind.Complete,
                cancellationToken: cancellationTokenSource.Token);

            await renderGateWaitStarted.Task.WaitAsync(TestHostSettings.DefaultTestTimeout);
            await cancellationTokenSource.CancelAsync();

            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await flush.WaitAsync(TestHostSettings.DefaultTestTimeout));
        }
        finally
        {
            releaseLock.TrySetResult();
            await lockHolder;
        }

        await Assert.That(buffer.HasOutput).IsTrue();
    }

    [Test]
    public async Task IncrementalFlush_CancelledRenderGateWait_DoesNotMarkRetryAsContinued()
    {
        var writer = new StringWriter();
        var renderGateWaitStarted = new TaskCompletionSource(
            TaskCreationOptions.RunContinuationsAsynchronously);
        var loggerControl = new SynchronousLoggerControl(writer)
        {
            RenderGateWaitStarted = renderGateWaitStarted,
        };
        var buffer = CreateBufferWithStructuredLog();
        var lockAcquired = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var releaseLock = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var lockHolder = Task.Run(() =>
        {
            lock (loggerControl.SynchronizationLock)
            {
                lockAcquired.TrySetResult();
                releaseLock.Task.GetAwaiter().GetResult();
            }
        });

        await lockAcquired.Task.WaitAsync(TestHostSettings.DefaultTestTimeout);
        using var cancellationTokenSource = new CancellationTokenSource();
        try
        {
            var flush = buffer.FlushToAsync(
                writer,
                new GitHubActionsFormatter(),
                loggerControl,
                loggerControl,
                OutputFlushKind.Incremental,
                cancellationToken: cancellationTokenSource.Token);

            await renderGateWaitStarted.Task.WaitAsync(TestHostSettings.DefaultTestTimeout);
            await cancellationTokenSource.CancelAsync();

            await Assert.ThrowsAsync<OperationCanceledException>(async () =>
                await flush.WaitAsync(TestHostSettings.DefaultTestTimeout));
        }
        finally
        {
            releaseLock.TrySetResult();
            await lockHolder;
        }

        await buffer.FlushToAsync(
            writer,
            new GitHubActionsFormatter(),
            loggerControl,
            loggerControl,
            OutputFlushKind.Incremental);

        var output = writer.ToString();
        await Assert.That(output).Contains("ModuleOutputBufferTests … (");
        await Assert.That(output).DoesNotContain("(continued)");
    }

    [Test]
    public async Task Flush_RenderGateTimeout_WritesBufferedOutputDirectly()
    {
        var writer = new StringWriter();
        var loggerControl = new SynchronousLoggerControl(writer);
        var fallbackLogger = new RecordingLogger();
        var logException = new InvalidOperationException("structured secret failure");
        var buffer = CreateBufferWithStructuredLog(
            TimeSpan.FromMilliseconds(50),
            "structured secret",
            new RedactingSecretObfuscator(),
            logException,
            LogLevel.Warning);
        buffer.WriteLine("direct output");
        var lockAcquired = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var releaseLock = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var lockHolder = Task.Run(() =>
        {
            lock (loggerControl.SynchronizationLock)
            {
                lockAcquired.TrySetResult();
                releaseLock.Task.GetAwaiter().GetResult();
            }
        });

        await lockAcquired.Task.WaitAsync(TimeSpan.FromSeconds(1));
        try
        {
            var flush = Task.Run(() => buffer.FlushToAsync(
                writer,
                new GitHubActionsFormatter(),
                loggerControl,
                loggerControl,
                OutputFlushKind.Complete,
                [fallbackLogger],
                CancellationToken.None));

            await flush.WaitAsync(TimeSpan.FromSeconds(2));
        }
        finally
        {
            releaseLock.TrySetResult();
            await lockHolder;
        }

        var output = writer.ToString();
        await Assert.That(output).Contains("Timed out waiting for the console logger render gate");
        await Assert.That(output).Contains("[WARN] structured ***");
        await Assert.That(output).DoesNotContain("structured secret");
        await Assert.That(output).Contains(nameof(InvalidOperationException));
        await Assert.That(output).Contains("structured *** failure");
        await Assert.That(output).Contains("direct output");
        await Assert.That(loggerControl.LogCallCount).IsEqualTo(0);
        await Assert.That(fallbackLogger.Entries).HasSingleItem();
        await Assert.That(fallbackLogger.Entries[0].Exception).IsNotSameReferenceAs(logException);
        await Assert.That(fallbackLogger.Entries[0].Exception?.ToString()).DoesNotContain("secret");
        await Assert.That(buffer.HasOutput).IsFalse();
    }

    [Test]
    public async Task Flush_NonSpectreConsole_WritesStructuredOutputSynchronously()
    {
        var writer = new StringWriter();
        var filterOptions = new LoggerFilterOptions();
        var optionsMonitor = Mock.Of<IOptionsMonitor<LoggerFilterOptions>>(options =>
            options.CurrentValue == filterOptions);
        var loggerControl = new NoopSpectreConsoleLoggerControl(
            NullLoggerFactory.Instance,
            optionsMonitor);
        var fallbackLogger = new SynchronousConsoleRecordingLogger(writer);
        var buffer = CreateBufferWithStructuredLog();

        await buffer.FlushToAsync(
            writer,
            new GitHubActionsFormatter(),
            NullLogger.Instance,
            loggerControl,
            OutputFlushKind.Complete,
            [fallbackLogger]);

        var output = writer.ToString();
        using (Assert.Multiple())
        {
            await Assert.That(output).Contains("formatted: structured log");
            await Assert.That(output).DoesNotContain("[INFO] structured log");
            await Assert.That(output).DoesNotContain("Timed out waiting");
            await Assert.That(output.IndexOf("formatted: structured log", StringComparison.Ordinal))
                .IsLessThan(output.IndexOf("::endgroup::", StringComparison.Ordinal));
        }
    }

    [Test]
    public async Task Flush_ExclusiveLoggerDisablesOutputGroup()
    {
        var writer = new StringWriter();
        var logger = new ExclusiveRecordingLogger(writer, isEnabled: false);
        var buffer = CreateBufferWithStructuredLog(isSpectreEnabled: static _ => true);

        await buffer.FlushToAsync(
            writer,
            new GitHubActionsFormatter(),
            logger,
            new SynchronousLoggerControl(writer),
            OutputFlushKind.Complete,
            [logger]);

        using (Assert.Multiple())
        {
            await Assert.That(writer.ToString()).DoesNotContain("::group::");
            await Assert.That(writer.ToString()).DoesNotContain("structured log");
            await Assert.That(logger.Entries).IsEmpty();
        }
    }

    [Test]
    public async Task Flush_ExclusiveLoggerEnablesOutputGroup()
    {
        var writer = new StringWriter();
        var logger = new ExclusiveRecordingLogger(writer, isEnabled: true);
        var buffer = CreateBufferWithStructuredLog(isSpectreEnabled: static _ => false);

        await buffer.FlushToAsync(
            writer,
            new GitHubActionsFormatter(),
            logger,
            new SynchronousLoggerControl(writer),
            OutputFlushKind.Complete,
            [logger]);

        var output = writer.ToString();
        using (Assert.Multiple())
        {
            await Assert.That(output).Contains("::group::");
            await Assert.That(output).Contains("structured log");
            await Assert.That(output.IndexOf("::group::", StringComparison.Ordinal))
                .IsLessThan(output.IndexOf("structured log", StringComparison.Ordinal));
            await Assert.That(output.IndexOf("structured log", StringComparison.Ordinal))
                .IsLessThan(output.IndexOf("::endgroup::", StringComparison.Ordinal));
        }
    }

    [Test]
    public async Task Flush_RenderGateTimeout_Respects_Spectre_Filter()
    {
        var writer = new StringWriter();
        var loggerControl = new SynchronousLoggerControl(writer);
        var fallbackLogger = new RecordingLogger();
        var buffer = CreateBufferWithStructuredLog(
            TimeSpan.FromMilliseconds(50),
            "filtered structured log",
            isSpectreEnabled: static _ => false);
        buffer.WriteLine("direct output");
        var lockAcquired = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var releaseLock = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var lockHolder = Task.Run(() =>
        {
            lock (loggerControl.SynchronizationLock)
            {
                lockAcquired.TrySetResult();
                releaseLock.Task.GetAwaiter().GetResult();
            }
        });

        await lockAcquired.Task.WaitAsync(TimeSpan.FromSeconds(1));
        try
        {
            await buffer.FlushToAsync(
                writer,
                new GitHubActionsFormatter(),
                loggerControl,
                loggerControl,
                OutputFlushKind.Complete,
                [fallbackLogger],
                CancellationToken.None);
        }
        finally
        {
            releaseLock.TrySetResult();
            await lockHolder;
        }

        var output = writer.ToString();
        await Assert.That(output).Contains("Timed out waiting for the console logger render gate");
        await Assert.That(output).Contains("direct output");
        await Assert.That(output).DoesNotContain("filtered structured log");
        await Assert.That(fallbackLogger.Entries).HasSingleItem();
        await Assert.That(buffer.HasOutput).IsFalse();
    }

    [Test]
    public async Task Flush_RenderGateTimeout_RetriesFailedProviderWithoutRepeatingConsoleOutput()
    {
        var writer = new StringWriter();
        var loggerControl = new SynchronousLoggerControl(writer);
        var successfulLogger = new RecordingLogger();
        var fallbackLogger = new RecordingLogger
        {
            LogException = new InvalidOperationException("provider rejected event"),
        };
        var buffer = CreateBufferWithStructuredLog(
            TimeSpan.FromMilliseconds(50),
            "retry structured log");
        var lockAcquired = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var releaseLock = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        var lockHolder = Task.Run(() =>
        {
            lock (loggerControl.SynchronizationLock)
            {
                lockAcquired.TrySetResult();
                releaseLock.Task.GetAwaiter().GetResult();
            }
        });

        await lockAcquired.Task.WaitAsync(TimeSpan.FromSeconds(1));
        try
        {
            await buffer.FlushToAsync(
                writer,
                new GitHubActionsFormatter(),
                loggerControl,
                loggerControl,
                OutputFlushKind.Complete,
                [successfulLogger, fallbackLogger],
                CancellationToken.None);
        }
        finally
        {
            releaseLock.TrySetResult();
            await lockHolder;
        }

        await Assert.That(buffer.HasOutput).IsTrue();
        await Assert.That(successfulLogger.Entries).HasSingleItem();
        await Assert.That(fallbackLogger.Entries).IsEmpty();
        await Assert.That(CountOccurrences(writer.ToString(), "[INFO] retry structured log"))
            .IsEqualTo(1);

        fallbackLogger.LogException = null;
        await buffer.FlushToAsync(
            writer,
            new GitHubActionsFormatter(),
            loggerControl,
            loggerControl,
            OutputFlushKind.Complete,
            [successfulLogger, fallbackLogger],
            CancellationToken.None);

        await Assert.That(buffer.HasOutput).IsFalse();
        await Assert.That(successfulLogger.Entries).HasSingleItem();
        await Assert.That(fallbackLogger.Entries).HasSingleItem();
        await Assert.That(CountOccurrences(writer.ToString(), "[INFO] retry structured log"))
            .IsEqualTo(1);
    }

    private static ModuleOutputBuffer CreateBufferWithStructuredLog(
        TimeSpan? renderGateTimeout = null,
        string message = "structured log",
        ISecretObfuscator? secretObfuscator = null,
        Exception? exception = null,
        LogLevel logLevel = LogLevel.Information,
        Func<LogLevel, bool>? isSpectreEnabled = null)
    {
        var buffer = new ModuleOutputBuffer(
            typeof(ModuleOutputBufferTests),
            renderGateTimeout: renderGateTimeout,
            isSpectreEnabled: isSpectreEnabled);
        buffer.AddLogEvent(new BufferedLogEvent<string>(
            logLevel,
            default,
            message,
            message,
            exception,
            static (state, _) => state,
            secretObfuscator ?? new PassthroughSecretObfuscator()));
        return buffer;
    }

    private static int CountOccurrences(string value, string search)
    {
        return value.Split(search, StringSplitOptions.None).Length - 1;
    }

    private sealed class PassthroughSecretObfuscator : ISecretObfuscator
    {
        public string Obfuscate(string? input, object? optionsObject) => input ?? string.Empty;
    }

    private sealed class RedactingSecretObfuscator : ISecretObfuscator
    {
        public string Obfuscate(string? input, object? optionsObject)
            => input?.Replace("secret", "***", StringComparison.Ordinal) ?? string.Empty;
    }

    private sealed class ThrowingEnumerableLogState
        : IEnumerable<KeyValuePair<string, object?>>
    {
        public IEnumerator<KeyValuePair<string, object?>> GetEnumerator() =>
            throw new InvalidOperationException("State must not be enumerated during buffering.");

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }

    private sealed class RecordingLogger : ILogger
    {
        public List<(string Message, Exception? Exception)> Entries { get; } = [];

        public List<Type> StateTypes { get; } = [];

        public Exception? LogException { get; set; }

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull
            => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (LogException is not null)
            {
                throw LogException;
            }

            StateTypes.Add(typeof(TState));
            Entries.Add((formatter(state, exception), exception));
        }
    }

    private sealed class SynchronousConsoleRecordingLogger(TextWriter writer)
        : ILogger, ISynchronousConsoleLogger
    {
        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter) =>
            writer.WriteLine($"formatted: {formatter(state, exception)}");
    }

    private sealed class ExclusiveRecordingLogger(TextWriter writer, bool isEnabled)
        : IExclusiveStructuredLogSink
    {
        public List<string> Entries { get; } = [];

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => isEnabled;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            var message = formatter(state, exception);
            Entries.Add(message);
            writer.WriteLine(message);
        }
    }

    private sealed class SynchronousLoggerControl(TextWriter writer) : ILogger, ISpectreConsoleLoggerControl
    {
        public object SynchronizationLock { get; } = new();

        public Action? AfterLog { get; set; }

        public Exception? LogException { get; set; }

        public Exception? LogExceptionAfterWrite { get; set; }

        public TaskCompletionSource? RenderGateWaitStarted { get; init; }

        public int LogCallCount { get; private set; }

        public IDisposable? BeginScope<TState>(TState state)
            where TState : notnull
            => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception? exception,
            Func<TState, Exception?, string> formatter)
        {
            LogCallCount++;

            if (LogException is not null)
            {
                throw LogException;
            }

            Write(formatter(state, exception));
            AfterLog?.Invoke();

            if (LogExceptionAfterWrite is not null)
            {
                throw LogExceptionAfterWrite;
            }
        }

        public void Write(string message)
        {
            lock (SynchronizationLock)
            {
                writer.WriteLine(message);
            }
        }

        public bool TryWrite(string message, TimeSpan timeout)
        {
            if (!Monitor.TryEnter(SynchronizationLock, timeout))
            {
                return false;
            }

            try
            {
                writer.WriteLine(message);
                return true;
            }
            finally
            {
                Monitor.Exit(SynchronizationLock);
            }
        }

        public IDisposable Suspend() => GateLease.Instance;

        public bool WouldRender(string categoryName, LogLevel logLevel) => true;

        public bool TryAcquireRenderGate(TimeSpan timeout, out IDisposable? gate)
        {
            if (!Monitor.TryEnter(SynchronizationLock, timeout))
            {
                gate = null;
                return false;
            }

            Monitor.Exit(SynchronizationLock);
            gate = GateLease.Instance;
            return true;
        }

        public async ValueTask<IDisposable?> TryAcquireRenderGateAsync(
            TimeSpan timeout,
            CancellationToken cancellationToken = default)
        {
            RenderGateWaitStarted?.TrySetResult();
            var deadline = DateTime.UtcNow + timeout;
            while (DateTime.UtcNow < deadline)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (Monitor.TryEnter(SynchronizationLock))
                {
                    Monitor.Exit(SynchronizationLock);
                    return GateLease.Instance;
                }

                var remaining = deadline - DateTime.UtcNow;
                if (remaining <= TimeSpan.Zero)
                {
                    return null;
                }

                await Task.Delay(
                        remaining < TimeSpan.FromMilliseconds(10)
                            ? remaining
                            : TimeSpan.FromMilliseconds(10),
                        cancellationToken)
                    .ConfigureAwait(false);
            }

            return null;
        }

        public Task FlushAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

        private sealed class GateLease : IDisposable
        {
            public static GateLease Instance { get; } = new();

            public void Dispose()
            {
            }
        }
    }

    private sealed class MarkupLikeBuildSystemFormatter : IBuildSystemFormatter
    {
        public bool UsesRawCommands => true;

        public string GetStartBlockCommand(string name) => "[red]::group::[/]";

        public string GetEndBlockCommand(string name) => "::endgroup::";

        public string? GetMaskSecretCommand(string secret) => null;
    }
}
