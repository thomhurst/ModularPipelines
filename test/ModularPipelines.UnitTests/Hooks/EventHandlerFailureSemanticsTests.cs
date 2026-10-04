using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Events;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Hooks;

/// <summary>
/// Verifies how handler failures affect module outcomes: gate handlers (Ready, Start) fail the module
/// through the normal failure path, while observer handlers (End, Failure, Skipped) never change it.
/// </summary>
[TUnit.Core.NotInParallel(nameof(EventHandlerFailureSemanticsTests))]
public class EventHandlerFailureSemanticsTests : TestBase
{
    private static readonly List<string> Log = [];
    private static readonly object LogLock = new();

    private static void Record(string entry)
    {
        lock (LogLock)
        {
            Log.Add(entry);
        }
    }

    private static List<string> Snapshot()
    {
        lock (LogLock)
        {
            return [.. Log];
        }
    }

    [Before(Test)]
    public void Reset()
    {
        lock (LogLock)
        {
            Log.Clear();
        }
    }

    [Test]
    public async Task Global_Ready_Handler_Failure_Fails_Module_And_Notifies_Failure_Handlers()
    {
        var summary = await CreateContinueOnFailureBuilder()
            .AddModuleEventHandler<ThrowingGlobalReadyHandler>()
            .AddModule<RecordingModule>()
            .RunAsync();

        var result = GetResult<RecordingModule>(summary);
        using (Assert.Multiple())
        {
            await Assert.That(result.Status).IsEqualTo(ModuleStatus.Failed);
            await Assert.That(result.ExceptionOrDefault?.Message).IsEqualTo("global ready failed");
            await Assert.That(Snapshot()).Contains("attribute-failure:RecordingModule");
            await Assert.That(Snapshot()).DoesNotContain("execute:RecordingModule");
        }
    }

    [Test]
    public async Task Global_Start_Handler_Failure_Fails_Module_And_Notifies_Failure_Handlers()
    {
        var summary = await CreateContinueOnFailureBuilder()
            .AddModuleEventHandler<ThrowingGlobalStartHandler>()
            .AddModule<RecordingModule>()
            .RunAsync();

        var result = GetResult<RecordingModule>(summary);
        using (Assert.Multiple())
        {
            await Assert.That(result.Status).IsEqualTo(ModuleStatus.Failed);
            await Assert.That(result.ExceptionOrDefault?.Message).IsEqualTo("global start failed");
            await Assert.That(Snapshot()).Contains("attribute-failure:RecordingModule");
            await Assert.That(Snapshot()).DoesNotContain("execute:RecordingModule");
        }
    }

    [Test]
    public async Task Throwing_End_Handler_Does_Not_Change_Succeeded_Outcome_And_Next_Family_Runs()
    {
        var summary = await CreateContinueOnFailureBuilder()
            .AddModuleEventHandler<RecordingGlobalHandler>()
            .AddModule<ThrowingEndModule>()
            .RunAsync();

        var result = GetResult<ThrowingEndModule>(summary);
        using (Assert.Multiple())
        {
            await Assert.That(result.Status).IsEqualTo(ModuleStatus.Succeeded);
            await Assert.That(Snapshot()).Contains("global-end:ThrowingEndModule");
            await Assert.That(Snapshot()).DoesNotContain("global-failure:ThrowingEndModule");
        }
    }

    [Test]
    public async Task Throwing_End_Handler_Is_Surfaced_As_Pipeline_Error()
    {
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            TestPipelineBuilder.Create()
                .AddModule<ThrowingEndModule>()
                .RunAsync());

        await Assert.That(exception!.Message).IsEqualTo("end handler failed");
    }

    [Test]
    public async Task Throwing_Failure_Handler_Keeps_Module_Exception_And_Runs_Global_Handlers()
    {
        var summary = await CreateContinueOnFailureBuilder()
            .AddModuleEventHandler<RecordingGlobalHandler>()
            .AddModule<ThrowingFailureHandlerModule>()
            .RunAsync();

        var result = GetResult<ThrowingFailureHandlerModule>(summary);
        using (Assert.Multiple())
        {
            await Assert.That(result.Status).IsEqualTo(ModuleStatus.Failed);
            await Assert.That(result.ExceptionOrDefault?.Message).IsEqualTo("module failed");
            await Assert.That(Snapshot()).Contains("global-failure:ThrowingFailureHandlerModule");
        }
    }

    [Test]
    public async Task Skipped_Handlers_Receive_The_Result_And_Cannot_Change_The_Skip()
    {
        var summary = await CreateContinueOnFailureBuilder()
            .AddModuleEventHandler<RecordingGlobalHandler>()
            .AddModule<ThrowingSkippedHandlerModule>()
            .RunAsync();

        var result = GetResult<ThrowingSkippedHandlerModule>(summary);
        using (Assert.Multiple())
        {
            await Assert.That(result.Status).IsEqualTo(ModuleStatus.Skipped);
            await Assert.That(Snapshot()).Contains("attribute-skipped-result:Skipped");
            await Assert.That(Snapshot()).Contains("global-skipped-result:Skipped");
        }
    }

    [Test]
    public async Task Handler_Registered_For_Both_Pipeline_And_Module_Events_Is_One_Instance()
    {
        await using var pipeline = await TestPipelineBuilder.Create()
            .AddModuleEventHandler<CombinedHandler>()
            .AddPipelineEventHandler<CombinedHandler>()
            .AddModule<RecordingModule>()
            .BuildAsync();

        var moduleHandler = pipeline.Services.GetServices<IModuleEventHandler>().OfType<CombinedHandler>().Single();
        var pipelineHandler = pipeline.Services.GetServices<IPipelineEventHandler>().OfType<CombinedHandler>().Single();

        await Assert.That(moduleHandler).IsSameReferenceAs(pipelineHandler);
    }

    private static PipelineBuilder CreateContinueOnFailureBuilder() =>
        TestPipelineBuilder.Create()
            .ConfigureOptions(options => options with
            {
                FailureMode = FailureMode.ContinueOnFailure,
                ThrowOnPipelineFailure = false,
            });

    private static IModuleResult GetResult<TModule>(PipelineSummary summary) =>
        summary.Results.Single(result => result.TypeName == typeof(TModule).FullName);

    public sealed class RecordFailureAttribute : Attribute, IModuleFailureHandler
    {
        public Task OnModuleFailureAsync(IModuleHookContext context, Exception exception, CancellationToken cancellationToken)
        {
            Record($"attribute-failure:{context.ModuleName}");
            return Task.CompletedTask;
        }
    }

    public sealed class ThrowingEndAttribute : Attribute, IModuleEndHandler
    {
        public Task OnModuleEndAsync(IModuleHookContext context, IModuleResult result, CancellationToken cancellationToken) =>
            throw new InvalidOperationException("end handler failed");
    }

    public sealed class ThrowingFailureAttribute : Attribute, IModuleFailureHandler
    {
        public Task OnModuleFailureAsync(IModuleHookContext context, Exception exception, CancellationToken cancellationToken) =>
            throw new InvalidOperationException("failure handler failed");
    }

    public sealed class ThrowingSkippedAttribute : Attribute, IModuleSkippedHandler
    {
        public Task OnModuleSkippedAsync(IModuleHookContext context, SkipDecision reason, CancellationToken cancellationToken)
        {
            Record($"attribute-skipped-result:{context.Result?.Status}");
            throw new InvalidOperationException("skipped handler failed");
        }
    }

    public sealed class ThrowingGlobalReadyHandler : IModuleEventHandler
    {
        public Task OnModuleReadyAsync(IModuleHookContext context, CancellationToken cancellationToken) =>
            throw new InvalidOperationException("global ready failed");
    }

    public sealed class ThrowingGlobalStartHandler : IModuleEventHandler
    {
        public Task OnModuleStartAsync(IModuleHookContext context, CancellationToken cancellationToken) =>
            throw new InvalidOperationException("global start failed");
    }

    public sealed class RecordingGlobalHandler : IModuleEventHandler
    {
        public Task OnModuleEndAsync(IModuleHookContext context, IModuleResult result, CancellationToken cancellationToken)
        {
            Record($"global-end:{context.ModuleName}");
            return Task.CompletedTask;
        }

        public Task OnModuleFailureAsync(IModuleHookContext context, Exception exception, CancellationToken cancellationToken)
        {
            Record($"global-failure:{context.ModuleName}");
            return Task.CompletedTask;
        }

        public Task OnModuleSkippedAsync(IModuleHookContext context, SkipDecision reason, CancellationToken cancellationToken)
        {
            Record($"global-skipped-result:{context.Result?.Status}");
            return Task.CompletedTask;
        }
    }

    public sealed class CombinedHandler : IModuleEventHandler, IPipelineEventHandler;

    [RecordFailure]
    public sealed class RecordingModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            Record($"execute:{nameof(RecordingModule)}");
            return Task.FromResult("done");
        }
    }

    [ThrowingEnd]
    public sealed class ThrowingEndModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult("done");
    }

    [ThrowingFailure]
    public sealed class ThrowingFailureHandlerModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            throw new InvalidOperationException("module failed");
    }

    [ThrowingSkipped]
    public sealed class ThrowingSkippedHandlerModule : Module<string>
    {
        protected override void Configure(ModuleConfigurationBuilder module) => module
            .WithSkipWhen(_ => SkipDecision.Skip("skip for test"));

        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
            Task.FromResult("unused");
    }
}
