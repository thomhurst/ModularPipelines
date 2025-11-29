using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Modules.Behaviors;
using ModularPipelines.TestHelpers;
using Polly;
using Polly.Retry;
using TUnit.Assertions.Extensions;

namespace ModularPipelines.UnitTests;

/// <summary>
/// Tests for the composition-based module pattern using behavior interfaces.
/// Modules implement IModule&lt;T&gt; via Module&lt;T&gt; and add behaviors via interfaces.
/// </summary>
public class ComposableModuleTests
{
    /// <summary>
    /// Example module using composition for skip behavior - always skips.
    /// Inherits from Module&lt;T&gt; and implements ISkippable for skip behavior.
    /// </summary>
    private class AlwaysSkippedModule : Module<string>, ISkippable
    {
        Task<SkipDecision> ISkippable.ShouldSkip(IPipelineContext context)
        {
            return Task.FromResult(SkipDecision.Skip("Skipped via composition"));
        }

        protected override Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<string?>("Executed");
        }
    }

    /// <summary>
    /// Example module using composition for skip behavior - never skips.
    /// Inherits from Module&lt;T&gt; and implements ISkippable for skip behavior.
    /// </summary>
    private class NeverSkippedModule : Module<string>, ISkippable
    {
        Task<SkipDecision> ISkippable.ShouldSkip(IPipelineContext context)
        {
            return Task.FromResult(SkipDecision.DoNotSkip);
        }

        protected override Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<string?>("Executed");
        }
    }

    /// <summary>
    /// Example module using composition for timeout behavior.
    /// Inherits from Module&lt;T&gt; and implements ITimeoutable for timeout behavior.
    /// </summary>
    private class TimeoutableModule : Module<string>, ITimeoutable
    {
        TimeSpan ITimeoutable.Timeout => TimeSpan.FromSeconds(5);

        protected override Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<string?>("Executed with timeout");
        }
    }

    /// <summary>
    /// Example module using composition for multiple behaviors.
    /// Demonstrates combining multiple behavior interfaces on a single module.
    /// </summary>
    private class MultiBehaviorModule : Module<int>, ISkippable, ITimeoutable, IHookable
    {
        public static bool BeforeHookCalled { get; private set; }
        public static bool AfterHookCalled { get; private set; }

        TimeSpan ITimeoutable.Timeout => TimeSpan.FromMinutes(1);

        Task<SkipDecision> ISkippable.ShouldSkip(IPipelineContext context)
        {
            return Task.FromResult(SkipDecision.DoNotSkip);
        }

        Task IHookable.OnBeforeExecute(IPipelineContext context)
        {
            BeforeHookCalled = true;
            return Task.CompletedTask;
        }

        Task IHookable.OnAfterExecute(IPipelineContext context)
        {
            AfterHookCalled = true;
            return Task.CompletedTask;
        }

        protected override Task<int> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(42);
        }

        public static void Reset()
        {
            BeforeHookCalled = false;
            AfterHookCalled = false;
        }
    }

    /// <summary>
    /// Example module that always runs using composition.
    /// Inherits from Module&lt;T&gt; and implements IAlwaysRun marker interface.
    /// </summary>
    private class AlwaysRunModule : Module<string>, IAlwaysRun
    {
        protected override Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<string?>("Always ran");
        }
    }

    [Test]
    public async Task Skippable_Module_Is_Skipped_When_Condition_True()
    {
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<AlwaysSkippedModule>()
            .ExecutePipelineAsync();

        var moduleResult = result.Modules.OfType<AlwaysSkippedModule>().Single();
        await Assert.That(moduleResult.SkipResult.ShouldSkip).IsTrue();
        await Assert.That(moduleResult.SkipResult.Reason).IsEqualTo("Skipped via composition");
    }

    [Test]
    public async Task Skippable_Module_Executes_When_Condition_False()
    {
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<NeverSkippedModule>()
            .ExecutePipelineAsync();

        var moduleResult = result.Modules.OfType<NeverSkippedModule>().Single();
        await Assert.That(moduleResult.SkipResult.ShouldSkip).IsFalse();
    }

    [Test]
    public async Task Timeoutable_Module_Has_Custom_Timeout()
    {
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<TimeoutableModule>()
            .ExecutePipelineAsync();

        var module = result.Modules.OfType<TimeoutableModule>().Single();
        // The module should have executed successfully with the custom timeout
        await Assert.That(module.Status).IsEqualTo(ModularPipelines.Enums.Status.Successful);
    }

    [Test]
    public async Task Multi_Behavior_Module_Calls_Hooks()
    {
        MultiBehaviorModule.Reset();

        var result = await TestPipelineHostBuilder.Create()
            .AddModule<MultiBehaviorModule>()
            .ExecutePipelineAsync();

        await Assert.That(MultiBehaviorModule.BeforeHookCalled).IsTrue();
        await Assert.That(MultiBehaviorModule.AfterHookCalled).IsTrue();
    }

    [Test]
    public async Task AlwaysRun_Module_Has_Correct_RunType()
    {
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<AlwaysRunModule>()
            .ExecutePipelineAsync();

        var module = result.Modules.OfType<AlwaysRunModule>().Single();
        await Assert.That(module.ModuleRunType).IsEqualTo(ModuleRunType.AlwaysRun);
    }
}
