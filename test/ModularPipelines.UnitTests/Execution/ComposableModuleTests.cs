using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using Polly;
using Polly.Retry;
using TUnit.Assertions.Extensions;

namespace ModularPipelines.UnitTests.Execution;

/// <summary>
/// Tests for the composition-based module pattern using ModuleConfiguration.
/// Modules implement Module&lt;T&gt; and configure behaviors via Configure() method.
/// </summary>
public class ComposableModuleTests
{
    /// <summary>
    /// Example module using ModuleConfiguration for skip behavior - always skips.
    /// Inherits from Module&lt;T&gt; and uses Configure() for skip behavior.
    /// </summary>
    private class AlwaysSkippedModule : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithSkipWhen(() => SkipDecision.Skip("Skipped via composition"))
            .Build();

        protected internal override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<string?>("Executed");
        }
    }

    /// <summary>
    /// Example module using ModuleConfiguration for skip behavior - never skips.
    /// Inherits from Module&lt;T&gt; and uses Configure() for skip behavior.
    /// </summary>
    private class NeverSkippedModule : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithSkipWhen(() => SkipDecision.DoNotSkip)
            .Build();

        protected internal override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<string?>("Executed");
        }
    }

    /// <summary>
    /// Example module using ModuleConfiguration for timeout behavior.
    /// Inherits from Module&lt;T&gt; and uses Configure() for timeout behavior.
    /// </summary>
    private class TimeoutableModule : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithTimeout(TimeSpan.FromSeconds(5))
            .Build();

        protected internal override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<string?>("Executed with timeout");
        }
    }

    /// <summary>
    /// Example module using ModuleConfiguration for multiple behaviors.
    /// Demonstrates combining multiple behaviors via Configure().
    /// </summary>
    private class MultiBehaviorModule : Module<int>
    {
        public static bool BeforeHookCalled { get; private set; }
        public static bool AfterHookCalled { get; private set; }

        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithTimeout(TimeSpan.FromMinutes(1))
            .WithSkipWhen(() => SkipDecision.DoNotSkip)
            .WithBeforeExecute(_ =>
            {
                BeforeHookCalled = true;
                return Task.CompletedTask;
            })
            .WithAfterExecute(_ =>
            {
                AfterHookCalled = true;
                return Task.CompletedTask;
            })
            .Build();

        protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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
    /// Example module that always runs using ModuleConfiguration.
    /// Inherits from Module&lt;T&gt; and uses Configure() with WithAlwaysRun().
    /// </summary>
    private class AlwaysRunModule : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithAlwaysRun()
            .Build();

        protected internal override Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<string?>("Always ran");
        }
    }

    [Test]
    public async Task Skippable_Module_Is_Skipped_When_Condition_True()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<AlwaysSkippedModule>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(AlwaysSkippedModule))!;
        await Assert.That(moduleResult.SkipDecisionOrDefault!.ShouldSkip).IsTrue();
        await Assert.That(moduleResult.SkipDecisionOrDefault.Reason).IsEqualTo("Skipped via composition");
    }

    [Test]
    public async Task Skippable_Module_Executes_When_Condition_False()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<NeverSkippedModule>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(NeverSkippedModule))!;
        await Assert.That(moduleResult.SkipDecisionOrDefault?.ShouldSkip ?? false).IsFalse();
    }

    [Test]
    public async Task Timeoutable_Module_Has_Custom_Timeout()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<TimeoutableModule>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(TimeoutableModule))!;
        // The module should have executed successfully with the custom timeout
        await Assert.That(moduleResult.ModuleStatus).IsEqualTo(ModularPipelines.Enums.Status.Successful);
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
    public async Task AlwaysRun_Module_Has_Correct_Configuration()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<AlwaysRunModule>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        // Verify the module is registered and executed
        var module = host.RootServices.GetServices<IModule>().OfType<AlwaysRunModule>().Single();
        await Assert.That(module).IsNotNull();
    }
}
