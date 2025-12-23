using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes.Events;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Modules.Behaviors;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Attributes;

[NotInParallel(nameof(LifecycleEventIntegrationTests))]
public class LifecycleEventIntegrationTests : TestBase
{
    private static readonly List<string> EventLog = new();

    public class LogStartAttribute : Attribute, IModuleStartEventReceiver
    {
        public Task OnModuleStartAsync(IModuleEventContext context)
        {
            EventLog.Add($"Start:{context.ModuleName}");
            return Task.CompletedTask;
        }
    }

    public class LogEndAttribute : Attribute, IModuleEndEventReceiver
    {
        public Task OnModuleEndAsync(IModuleEventContext context, ModuleResult result)
        {
            EventLog.Add($"End:{context.ModuleName}");
            return Task.CompletedTask;
        }
    }

    public class LogFailedAttribute : Attribute, IModuleFailureEventReceiver
    {
        public bool ContinueOnError => true;

        public Task OnModuleFailureAsync(IModuleEventContext context, Exception exception)
        {
            EventLog.Add($"Failed:{context.ModuleName}:{exception.Message}");
            return Task.CompletedTask;
        }
    }

    public class LogSkippedAttribute : Attribute, IModuleSkippedEventReceiver
    {
        public Task OnModuleSkippedAsync(IModuleEventContext context, SkipDecision reason)
        {
            EventLog.Add($"Skipped:{context.ModuleName}:{reason.Reason}");
            return Task.CompletedTask;
        }
    }

    [LogStart]
    [LogEnd]
    public class SuccessfulModule : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return "Success";
        }
    }

    [LogStart]
    [LogFailed]
    public class FailingModule : Module<string>
    {
        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            throw new InvalidOperationException("Intentional failure");
        }
    }

    [LogStart]
    [LogSkipped]
    public class SkippingModule : Module<string>, ISkippable
    {
        public Task<SkipDecision> ShouldSkip(IPipelineContext context)
        {
            return Task.FromResult(SkipDecision.Skip("Test skip reason"));
        }

        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<string?>("Should not execute");
        }
    }

    [Before(Test)]
    public void ClearEventLog()
    {
        EventLog.Clear();
    }

    [Test]
    public async Task SuccessfulModule_InvokesStartAndEndEvents()
    {
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<SuccessfulModule>()
            .ExecutePipelineAsync();

        await Assert.That(result.Status).IsEqualTo(Enums.Status.Successful);
        await Assert.That(EventLog).Contains("Start:SuccessfulModule");
        await Assert.That(EventLog).Contains("End:SuccessfulModule");
    }

    [Test]
    public async Task FailingModule_InvokesStartAndFailedEvents()
    {
        try
        {
            await TestPipelineHostBuilder.Create()
                .AddModule<FailingModule>()
                .ConfigurePipelineOptions((_, options) =>
                {
                    options.ExecutionMode = ExecutionMode.WaitForAllModules;
                })
                .ExecutePipelineAsync();
        }
        catch
        {
            // Expected - the pipeline throws when a module fails
        }

        await Assert.That(EventLog).Contains("Start:FailingModule");
        await Assert.That(EventLog.Any(e => e.StartsWith("Failed:FailingModule:"))).IsTrue();
    }

    [Test]
    public async Task SkippingModule_InvokesStartAndSkippedEvents()
    {
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<SkippingModule>()
            .ExecutePipelineAsync();

        await Assert.That(result.Status).IsEqualTo(Enums.Status.Successful);
        await Assert.That(EventLog).Contains("Start:SkippingModule");
        await Assert.That(EventLog.Any(e => e.Contains("Skipped:SkippingModule:Test skip reason"))).IsTrue();
    }
}
