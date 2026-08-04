using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Configuration;
using ModularPipelines.Conditions;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Hooks;

public class DirectModuleHooksTests : TestBase
{
    #region Test Modules

    /// <summary>
    /// Module that tracks which lifecycle hooks were called and in what order.
    /// </summary>
    private class HookTrackingModule : Module<string>
    {
        public List<string> HooksCalled { get; } = [];

        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            HooksCalled.Add("ExecuteAsync");
            return "Success";
        }

        protected override Task OnBeforeExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            HooksCalled.Add("OnBeforeExecuteAsync");
            return Task.CompletedTask;
        }

        protected override Task<ModuleResult<string>?> OnAfterExecuteAsync(
            IModuleContext context,
            ModuleResult<string> result,
            CancellationToken cancellationToken)
        {
            HooksCalled.Add("OnAfterExecuteAsync");
            return Task.FromResult<ModuleResult<string>?>(null);
        }
    }

    /// <summary>
    /// Module that modifies its result in OnAfterExecuteAsync.
    /// </summary>
    private class ResultModifyingModule : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return "Original";
        }

        protected override async Task<ModuleResult<string>?> OnAfterExecuteAsync(
            IModuleContext context,
            ModuleResult<string> result,
            CancellationToken cancellationToken)
        {
            await Task.Yield();
            return result is ModuleResult<string>.Success success
                ? success with { Value = "Transformed" }
                : null;
        }
    }

    /// <summary>
    /// Module that is skipped and tracks OnSkippedAsync.
    /// </summary>
    private class SkippableHookTrackingModule : Module<string>
    {
        public List<string> HooksCalled { get; } = [];
        public SkipDecision? ReceivedSkipDecision { get; private set; }

        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithSkipWhen(_ => SkipDecision.Skip("Test skip reason"))
            .Build();

        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            HooksCalled.Add("ExecuteAsync");
            return "Should not reach here";
        }

        protected override Task OnSkippedAsync(
            IModuleContext context,
            SkipDecision skipDecision,
            CancellationToken cancellationToken)
        {
            HooksCalled.Add("OnSkippedAsync");
            ReceivedSkipDecision = skipDecision;
            return Task.CompletedTask;
        }
    }

    private class AlwaysTrueCondition : IRunCondition
    {
        public Task<bool> EvaluateAsync(IPipelineContext context) => Task.FromResult(true);
    }

    [ModularPipelines.Attributes.SkipIf<AlwaysTrueCondition>]
    private class AttributeSkippableHookTrackingModule : Module<string>
    {
        public List<string> HooksCalled { get; } = [];
        public SkipDecision? ReceivedSkipDecision { get; private set; }

        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            HooksCalled.Add("ExecuteAsync");
            return Task.FromResult<string>("Should not reach here");
        }

        protected override Task OnSkippedAsync(
            IModuleContext context,
            SkipDecision skipDecision,
            CancellationToken cancellationToken)
        {
            HooksCalled.Add("OnSkippedAsync");
            ReceivedSkipDecision = skipDecision;
            return Task.CompletedTask;
        }
    }

    /// <summary>
    /// Module that fails and tracks OnFailedAsync.
    /// </summary>
    private class FailingHookTrackingModule : Module<string>
    {
        public List<string> HooksCalled { get; } = [];
        public Exception? ReceivedFailureException { get; private set; }
        public ModuleResult<string>? ReceivedAfterResult { get; private set; }

        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithIgnoreFailures()
            .Build();

        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            HooksCalled.Add("ExecuteAsync");
            throw new InvalidOperationException("Test failure");
        }

        protected override Task OnFailedAsync(
            IModuleContext context,
            Exception exception,
            CancellationToken cancellationToken)
        {
            HooksCalled.Add("OnFailedAsync");
            ReceivedFailureException = exception;
            return Task.CompletedTask;
        }

        protected override Task<ModuleResult<string>?> OnAfterExecuteAsync(
            IModuleContext context,
            ModuleResult<string> result,
            CancellationToken cancellationToken)
        {
            // OnAfterExecuteAsync is called for both success and failure (after OnFailedAsync for failures)
            HooksCalled.Add("OnAfterExecuteAsync");
            ReceivedAfterResult = result;
            return Task.FromResult<ModuleResult<string>?>(null);
        }
    }

    private class NonIgnoredFailingHookTrackingModule : FailingHookTrackingModule
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .Build();
    }

    /// <summary>
    /// Module that throws in OnBeforeExecuteAsync.
    /// </summary>
    private class BeforeHookFailingModule : Module<string>
    {
        public List<string> HooksCalled { get; } = [];

        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithIgnoreFailures()
            .Build();

        protected override Task OnBeforeExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            HooksCalled.Add("OnBeforeExecuteAsync");
            throw new InvalidOperationException("Before hook failure");
        }

        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            HooksCalled.Add("ExecuteAsync");
            return "Should not reach here";
        }
    }

    /// <summary>
    /// Module that throws in OnAfterExecuteAsync to verify result is preserved.
    /// </summary>
    private class AfterHookFailingModule : Module<string>
    {
        public List<string> HooksCalled { get; } = [];

        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            HooksCalled.Add("ExecuteAsync");
            return "Success";
        }

        protected override Task<ModuleResult<string>?> OnAfterExecuteAsync(
            IModuleContext context,
            ModuleResult<string> result,
            CancellationToken cancellationToken)
        {
            HooksCalled.Add("OnAfterExecuteAsync");
            throw new InvalidOperationException("After hook failure");
        }
    }

    private class SelfAwaitingAfterHookModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) => Task.FromResult<string>("Success");

        protected override async Task<ModuleResult<string>?> OnAfterExecuteAsync(
            IModuleContext context,
            ModuleResult<string> result,
            CancellationToken cancellationToken) => await this;
    }

    #endregion

    #region Tests

    [Test]
    public async Task OnBeforeExecuteAsync_CalledBeforeExecuteAsync()
    {
        var module = await RunModule<HookTrackingModule>();

        await Assert.That(module.HooksCalled).Contains("OnBeforeExecuteAsync");
        await Assert.That(module.HooksCalled).Contains("ExecuteAsync");

        var beforeIndex = module.HooksCalled.IndexOf("OnBeforeExecuteAsync");
        var executeIndex = module.HooksCalled.IndexOf("ExecuteAsync");
        await Assert.That(beforeIndex).IsLessThan(executeIndex);
    }

    [Test]
    public async Task OnAfterExecuteAsync_CalledAfterExecuteAsync()
    {
        var module = await RunModule<HookTrackingModule>();

        await Assert.That(module.HooksCalled).Contains("ExecuteAsync");
        await Assert.That(module.HooksCalled).Contains("OnAfterExecuteAsync");

        var executeIndex = module.HooksCalled.IndexOf("ExecuteAsync");
        var afterIndex = module.HooksCalled.IndexOf("OnAfterExecuteAsync");
        await Assert.That(executeIndex).IsLessThan(afterIndex);
    }

    [Test]
    [Timeout(30_000)]
    public async Task OnAfterExecuteAsync_Can_Await_Its_Own_Module(
        CancellationToken cancellationToken)
    {
        var module = await RunModule<SelfAwaitingAfterHookModule>()
            .WaitAsync(cancellationToken);
        var result = await module;

        await Assert.That(result.ValueOrDefault).IsEqualTo("Success");
    }

    [Test]
    public async Task OnAfterExecuteAsync_PublishesTransformedResult()
    {
        var module = await RunModule<ResultModifyingModule>();
        var result = await module;

        await Assert.That(result.ValueOrDefault).IsEqualTo("Transformed");
    }

    [Test]
    public async Task OnSkippedAsync_CalledWhenModuleSkipped()
    {
        var module = await RunModule<SkippableHookTrackingModule>();

        await Assert.That(module.HooksCalled).Contains("OnSkippedAsync");
        await Assert.That(module.HooksCalled).DoesNotContain("ExecuteAsync");
        await Assert.That(module.ReceivedSkipDecision).IsNotNull();
        await Assert.That(module.ReceivedSkipDecision!.Reason).IsEqualTo("Test skip reason");
    }

    [Test]
    public async Task OnSkippedAsync_CalledWhenAttributeSkipsModule()
    {
        var module = await RunModule<AttributeSkippableHookTrackingModule>();

        await Assert.That(module.HooksCalled).Contains("OnSkippedAsync");
        await Assert.That(module.HooksCalled).DoesNotContain("ExecuteAsync");
        await Assert.That(module.ReceivedSkipDecision).IsNotNull();
        await Assert.That(module.ReceivedSkipDecision!.Reason)
            .IsEqualTo("SkipIf<AlwaysTrueCondition> returned true");
    }

    [Test]
    public async Task OnFailedAsync_CalledWhenModuleFails()
    {
        var module = await RunModule<FailingHookTrackingModule>();

        await Assert.That(module.HooksCalled).Contains("ExecuteAsync");
        await Assert.That(module.HooksCalled).Contains("OnFailedAsync");
        await Assert.That(module.ReceivedFailureException).IsNotNull();
        await Assert.That(module.ReceivedFailureException).IsTypeOf<InvalidOperationException>();
    }

    [Test]
    public async Task OnAfterExecuteAsync_CalledWhenModuleFails()
    {
        var module = await RunModule<FailingHookTrackingModule>();

        // OnAfterExecuteAsync should be called even when module fails
        await Assert.That(module.HooksCalled).Contains("OnAfterExecuteAsync");

        // It should be called after OnFailedAsync
        var failedIndex = module.HooksCalled.IndexOf("OnFailedAsync");
        var afterIndex = module.HooksCalled.IndexOf("OnAfterExecuteAsync");
        await Assert.That(failedIndex).IsLessThan(afterIndex);

        // The result passed to OnAfterExecuteAsync should contain the exception
        await Assert.That(module.ReceivedAfterResult).IsNotNull();
        await Assert.That(module.ReceivedAfterResult!.ExceptionOrDefault).IsNotNull();
        await Assert.That(module.ReceivedAfterResult.ExceptionOrDefault).IsTypeOf<InvalidOperationException>();
    }

    [Test]
    public async Task OnAfterExecuteAsync_CalledWhenNonIgnoredModuleFails()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<NonIgnoredFailingHookTrackingModule>()
            .BuildAsync();
        var module = host.Services.GetServices<IModule>()
            .OfType<NonIgnoredFailingHookTrackingModule>()
            .Single();

        await Assert.ThrowsAsync<ModuleFailedException>(() => host.RunAsync());

        await Assert.That(module.HooksCalled).Contains("OnAfterExecuteAsync");
        await Assert.That(module.ReceivedAfterResult).IsNotNull();
        await Assert.That(module.ReceivedAfterResult!.ModuleStatus).IsEqualTo(Status.Failed);
        await Assert.That(module.ReceivedAfterResult.ExceptionOrDefault)
            .IsTypeOf<InvalidOperationException>();
    }

    [Test]
    public async Task OnBeforeExecuteAsync_ExceptionPreventsExecution()
    {
        var module = await RunModule<BeforeHookFailingModule>();

        await Assert.That(module.HooksCalled).Contains("OnBeforeExecuteAsync");
        await Assert.That(module.HooksCalled).DoesNotContain("ExecuteAsync");
    }

    [Test]
    public async Task OnAfterExecuteAsync_ExceptionLogged_ResultPreserved()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<AfterHookFailingModule>()
            .BuildAsync();

        await host.RunAsync();

        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(AfterHookFailingModule)) as ModuleResult<string>;

        // Module should still succeed despite after hook throwing
        await Assert.That(moduleResult).IsNotNull();
        await Assert.That(moduleResult!.ModuleStatus).IsEqualTo(Status.Successful);
        await Assert.That(moduleResult.ValueOrDefault).IsEqualTo("Success");
    }

    [Test]
    public async Task Module_WithNoOverrides_ExecutesNormally()
    {
        // A module that doesn't override any hooks should work fine
        var host = await TestPipelineBuilder.Create()
            .AddModule<ResultModifyingModule>()
            .BuildAsync();

        var result = await host.RunAsync();

        await Assert.That(result.Modules).Count().IsEqualTo(1);
    }

    #endregion
}
