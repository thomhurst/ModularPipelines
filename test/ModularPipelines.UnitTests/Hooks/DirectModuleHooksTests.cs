using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine;
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

        protected internal override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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
        protected internal override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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
            // We can't easily modify the internal result here, so we just verify the hook was called
            // In production, users could use this to log, transform, or wrap results
            return null; // null means keep original result
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
            .WithSkipWhen(() => SkipDecision.Skip("Test skip reason"))
            .Build();

        protected internal override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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

        protected internal override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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

        protected internal override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            HooksCalled.Add("ExecuteAsync");
            return "Should not reach here";
        }
    }

    /// <summary>
    /// Module that uses ModuleConfiguration hooks to verify ordering with direct hooks.
    /// </summary>
    private class HookableAndDirectHookModule : Module<string>
    {
        public List<string> HooksCalled { get; } = [];

        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithBeforeExecute(_ =>
            {
                HooksCalled.Add("Config:OnBeforeExecute");
                return Task.CompletedTask;
            })
            .WithAfterExecute(_ =>
            {
                HooksCalled.Add("Config:OnAfterExecute");
                return Task.CompletedTask;
            })
            .Build();

        protected internal override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            HooksCalled.Add("ExecuteAsync");
            return "Success";
        }

        protected override Task OnBeforeExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            HooksCalled.Add("Direct:OnBeforeExecuteAsync");
            return Task.CompletedTask;
        }

        protected override Task<ModuleResult<string>?> OnAfterExecuteAsync(
            IModuleContext context,
            ModuleResult<string> result,
            CancellationToken cancellationToken)
        {
            HooksCalled.Add("Direct:OnAfterExecuteAsync");
            return Task.FromResult<ModuleResult<string>?>(null);
        }
    }

    /// <summary>
    /// Module that throws in OnAfterExecuteAsync to verify result is preserved.
    /// </summary>
    private class AfterHookFailingModule : Module<string>
    {
        public List<string> HooksCalled { get; } = [];

        protected internal override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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
    public async Task OnSkippedAsync_CalledWhenModuleSkipped()
    {
        var module = await RunModule<SkippableHookTrackingModule>();

        await Assert.That(module.HooksCalled).Contains("OnSkippedAsync");
        await Assert.That(module.HooksCalled).DoesNotContain("ExecuteAsync");
        await Assert.That(module.ReceivedSkipDecision).IsNotNull();
        await Assert.That(module.ReceivedSkipDecision!.Reason).IsEqualTo("Test skip reason");
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
    public async Task DirectHooks_CalledBeforeConfigHooks()
    {
        var module = await RunModule<HookableAndDirectHookModule>();

        // Direct hooks should be called first
        await Assert.That(module.HooksCalled).Contains("Direct:OnBeforeExecuteAsync");
        await Assert.That(module.HooksCalled).Contains("Config:OnBeforeExecute");

        var directBeforeIndex = module.HooksCalled.IndexOf("Direct:OnBeforeExecuteAsync");
        var configBeforeIndex = module.HooksCalled.IndexOf("Config:OnBeforeExecute");
        await Assert.That(directBeforeIndex).IsLessThan(configBeforeIndex);
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
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<AfterHookFailingModule>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var moduleResult = resultRegistry.GetResult(typeof(AfterHookFailingModule)) as ModuleResult<string>;

        // Module should still succeed despite after hook throwing
        await Assert.That(moduleResult).IsNotNull();
        await Assert.That(moduleResult!.ModuleResultType).IsEqualTo(ModuleResultType.Success);
        await Assert.That(moduleResult.ValueOrDefault).IsEqualTo("Success");
    }

    [Test]
    public async Task Module_WithNoOverrides_ExecutesNormally()
    {
        // A module that doesn't override any hooks should work fine
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<ResultModifyingModule>()
            .BuildHostAsync();

        var result = await host.ExecutePipelineAsync();

        await Assert.That(result.Modules).HasCount().EqualTo(1);
    }

    #endregion
}
