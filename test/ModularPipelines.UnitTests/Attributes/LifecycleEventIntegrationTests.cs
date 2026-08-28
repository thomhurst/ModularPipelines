using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes.Events;
using ModularPipelines.Configuration;
using ModularPipelines.Conditions;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Attributes;

[NotInParallel(nameof(LifecycleEventIntegrationTests))]
public class LifecycleEventIntegrationTests : TestBase
{
    private static readonly List<string> EventLog = new();

    public class LogStartAttribute : Attribute, IModuleStartHandler
    {
        public Task OnModuleStartAsync(IModuleHookContext context)
        {
            EventLog.Add($"Start:{context.ModuleName}");
            return Task.CompletedTask;
        }
    }

    public class LogEndAttribute : Attribute, IModuleEndHandler
    {
        public Task OnModuleEndAsync(IModuleHookContext context, IModuleResult result)
        {
            EventLog.Add($"End:{context.ModuleName}");
            return Task.CompletedTask;
        }
    }

    public class LogFailedAttribute : Attribute, IModuleFailureHandler
    {
        public bool ContinueOnError => true;

        public Task OnModuleFailureAsync(IModuleHookContext context, Exception exception)
        {
            EventLog.Add($"Failed:{context.ModuleName}:{exception.Message}");
            return Task.CompletedTask;
        }
    }

    public class LogSkippedAttribute : Attribute, IModuleSkippedHandler
    {
        public Task OnModuleSkippedAsync(IModuleHookContext context, SkipDecision reason)
        {
            EventLog.Add($"Skipped:{context.ModuleName}:{reason.Reason}");
            return Task.CompletedTask;
        }
    }

    public class AlwaysTrueCondition : IRunCondition
    {
        public Task<bool> EvaluateAsync(IPipelineContext context) => Task.FromResult(true);
    }

    [LogStart]
    [LogEnd]
    public class SuccessfulModule : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return "Success";
        }
    }

    [LogStart]
    [LogFailed]
    public class FailingModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            throw new InvalidOperationException("Intentional failure");
        }
    }

    [LogStart]
    [LogSkipped]
    public class SkippingModule : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithSkipWhen(_ => SkipDecision.Skip("Test skip reason"))
            .Build();

        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<string>("Should not execute");
        }
    }

    [ModularPipelines.Attributes.SkipIf<AlwaysTrueCondition>]
    [LogStart]
    [LogSkipped]
    public class AttributeSkippingModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            return Task.FromResult<string>("Should not execute");
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
        var result = await TestPipelineBuilder.Create()
            .AddModule<SuccessfulModule>()
            .RunAsync();

        await Assert.That(result.Status).IsEqualTo(Enums.ModuleStatus.Succeeded);
        await Assert.That(EventLog).Contains("Start:SuccessfulModule");
        await Assert.That(EventLog).Contains("End:SuccessfulModule");
    }

    [Test]
    public async Task FailingModule_InvokesStartAndFailedEvents()
    {
        try
        {
            await TestPipelineBuilder.Create()
                .AddModule<FailingModule>()
                .ConfigurePipelineOptions((_, options) => options with
                {
                    ExecutionMode = ExecutionMode.WaitForAllModules,
                })
                .RunAsync();
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
        var result = await TestPipelineBuilder.Create()
            .AddModule<SkippingModule>()
            .RunAsync();

        await Assert.That(result.Status).IsEqualTo(Enums.ModuleStatus.Succeeded);
        await Assert.That(EventLog).Contains("Start:SkippingModule");
        await Assert.That(EventLog.Any(e => e.Contains("Skipped:SkippingModule:Test skip reason"))).IsTrue();
    }

    [Test]
    public async Task AttributeSkippingModule_InvokesStartAndSkippedEvents()
    {
        var result = await TestPipelineBuilder.Create()
            .AddModule<AttributeSkippingModule>()
            .RunAsync();

        await Assert.That(result.Status).IsEqualTo(Enums.ModuleStatus.Succeeded);
        await Assert.That(EventLog).Contains("Start:AttributeSkippingModule");
        await Assert.That(EventLog.Any(e => e.Contains("Skipped:AttributeSkippingModule:SkipIf<AlwaysTrueCondition> returned true"))).IsTrue();
    }
}
