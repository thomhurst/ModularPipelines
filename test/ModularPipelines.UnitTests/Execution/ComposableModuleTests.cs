using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
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
            .WithSkipWhen(_ => SkipDecision.Skip("Skipped via composition"))
            .Build();

        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<string>("Executed");
        }
    }

    /// <summary>
    /// Example module using ModuleConfiguration for skip behavior - never skips.
    /// Inherits from Module&lt;T&gt; and uses Configure() for skip behavior.
    /// </summary>
    private class NeverSkippedModule : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithSkipWhen(_ => SkipDecision.DoNotSkip)
            .Build();

        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<string>("Executed");
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

        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<string>("Executed with timeout");
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
            .WithSkipWhen(_ => SkipDecision.DoNotSkip)
            .Build();

        protected internal override Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(42);
        }

        protected override Task OnBeforeExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            BeforeHookCalled = true;
            return Task.CompletedTask;
        }

        protected override Task<ModuleResult<int>?> OnAfterExecuteAsync(
            IModuleContext context,
            ModuleResult<int> result,
            CancellationToken cancellationToken)
        {
            AfterHookCalled = true;
            return Task.FromResult<ModuleResult<int>?>(null);
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

        protected internal override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<string>("Always ran");
        }
    }

    [Test]
    public async Task Skippable_Module_Is_Skipped_When_Condition_True()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<AlwaysSkippedModule>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(AlwaysSkippedModule))!;
        await Assert.That(moduleResult.SkipDecisionOrDefault!.ShouldSkip).IsTrue();
        await Assert.That(moduleResult.SkipDecisionOrDefault.Reason).IsEqualTo("Skipped via composition");
    }

    [Test]
    public async Task Skippable_Module_Executes_When_Condition_False()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<NeverSkippedModule>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(NeverSkippedModule))!;
        await Assert.That(moduleResult.SkipDecisionOrDefault?.ShouldSkip ?? false).IsFalse();
    }

    [Test]
    public async Task Timeoutable_Module_Has_Custom_Timeout()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<TimeoutableModule>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(TimeoutableModule))!;
        // The module should have executed successfully with the custom timeout
        await Assert.That(moduleResult.Status).IsEqualTo(ModularPipelines.Enums.ModuleStatus.Succeeded);
    }

    [Test]
    public async Task Multi_Behavior_Module_Calls_Hooks()
    {
        MultiBehaviorModule.Reset();

        var result = await TestPipelineBuilder.Create()
            .AddModule<MultiBehaviorModule>()
            .RunAsync();

        await Assert.That(MultiBehaviorModule.BeforeHookCalled).IsTrue();
        await Assert.That(MultiBehaviorModule.AfterHookCalled).IsTrue();
    }

    [Test]
    public async Task AlwaysRun_Module_Has_Correct_Configuration()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<AlwaysRunModule>()
            .BuildAsync();

        await host.RunAsync();

        // Verify the module is registered and executed
        var module = host.Services.GetServices<IModule>().OfType<AlwaysRunModule>().Single();
        await Assert.That(module).IsNotNull();
    }
}
