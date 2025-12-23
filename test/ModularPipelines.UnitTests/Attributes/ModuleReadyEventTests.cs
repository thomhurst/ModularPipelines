using ModularPipelines.Attributes;
using ModularPipelines.Attributes.Events;
using ModularPipelines.Context;
using ModularPipelines.Enums;
using ModularPipelines.Modules;
using ModularPipelines.Modules.Behaviors;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Attributes;

[TUnit.Core.NotInParallel(nameof(ModuleReadyEventTests))]
public class ModuleReadyEventTests : TestBase
{
    private static readonly List<string> EventLog = new();

    public class LogReadyAttribute : Attribute, IModuleReadyEventReceiver
    {
        public Task OnModuleReadyAsync(IModuleReadyContext context)
        {
            EventLog.Add($"Ready:{context.ModuleName}");
            return Task.CompletedTask;
        }
    }

    public class LogReadyWithTimingAttribute : Attribute, IModuleReadyEventReceiver
    {
        public Task OnModuleReadyAsync(IModuleReadyContext context)
        {
            EventLog.Add($"Ready:{context.ModuleName}:WaitTime:{context.TimeWaitingForDependencies.TotalMilliseconds >= 0}");
            return Task.CompletedTask;
        }
    }

    public class LogReadyAndStartAttribute : Attribute, IModuleReadyEventReceiver, IModuleStartEventReceiver
    {
        public Task OnModuleReadyAsync(IModuleReadyContext context)
        {
            EventLog.Add($"Ready:{context.ModuleName}");
            return Task.CompletedTask;
        }

        public Task OnModuleStartAsync(IModuleEventContext context)
        {
            EventLog.Add($"Start:{context.ModuleName}");
            return Task.CompletedTask;
        }
    }

    public class ThrowingReadyAttribute : Attribute, IModuleReadyEventReceiver
    {
        public bool ContinueOnError => true;

        public Task OnModuleReadyAsync(IModuleReadyContext context)
        {
            throw new InvalidOperationException("Ready event failed");
        }
    }

    [LogReady]
    public class SimpleModuleWithReadyEvent : Module<string>
    {
        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<string?>("Done");
        }
    }

    [LogReadyWithTiming]
    public class ModuleWithTimingCheck : Module<string>
    {
        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<string?>("Done");
        }
    }

    [LogReadyAndStart]
    public class ModuleWithReadyAndStart : Module<string>
    {
        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<string?>("Done");
        }
    }

    [LogReady]
    [ModularPipelines.Attributes.DependsOn<DependencyModule>]
    public class DependentModuleWithReadyEvent : Module<string>
    {
        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<string?>("Dependent Done");
        }
    }

    public class DependencyModule : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Delay(10, cancellationToken);
            return "Dependency Done";
        }
    }

    [ThrowingReady]
    public class ModuleWithThrowingReadyEvent : Module<string>
    {
        public override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<string?>("Done");
        }
    }

    [Before(Test)]
    public void ClearEventLog()
    {
        EventLog.Clear();
    }

    [Test]
    public async Task ReadyEvent_IsFired_WhenModuleIsReady()
    {
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<SimpleModuleWithReadyEvent>()
            .ExecutePipelineAsync();

        await Assert.That(result.Status).IsEqualTo(Status.Successful);
        await Assert.That(EventLog).Contains("Ready:SimpleModuleWithReadyEvent");
    }

    [Test]
    public async Task ReadyEvent_ProvidesValidContext()
    {
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<ModuleWithTimingCheck>()
            .ExecutePipelineAsync();

        await Assert.That(result.Status).IsEqualTo(Status.Successful);
        await Assert.That(EventLog.Any(e => e.Contains("Ready:ModuleWithTimingCheck:WaitTime:True"))).IsTrue();
    }

    [Test]
    public async Task ReadyEvent_FiresBeforeStartEvent()
    {
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<ModuleWithReadyAndStart>()
            .ExecutePipelineAsync();

        await Assert.That(result.Status).IsEqualTo(Status.Successful);

        var readyIndex = EventLog.IndexOf("Ready:ModuleWithReadyAndStart");
        var startIndex = EventLog.IndexOf("Start:ModuleWithReadyAndStart");

        await Assert.That(readyIndex).IsGreaterThanOrEqualTo(0);
        await Assert.That(startIndex).IsGreaterThanOrEqualTo(0);
        await Assert.That(readyIndex).IsLessThan(startIndex);
    }

    [Test]
    public async Task ReadyEvent_IsFired_WhenDependenciesComplete()
    {
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<DependencyModule>()
            .AddModule<DependentModuleWithReadyEvent>()
            .ExecutePipelineAsync();

        await Assert.That(result.Status).IsEqualTo(Status.Successful);
        await Assert.That(EventLog).Contains("Ready:DependentModuleWithReadyEvent");
    }

    [Test]
    public async Task ReadyEvent_WithContinueOnError_DoesNotFailPipeline()
    {
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<ModuleWithThrowingReadyEvent>()
            .ExecutePipelineAsync();

        await Assert.That(result.Status).IsEqualTo(Status.Successful);
    }
}
