using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using Status = ModularPipelines.Enums.Status;

namespace ModularPipelines.UnitTests.Modules;

public class SyncModuleTests : TestBase
{
    #region Basic Execution Tests

    public class SimpleSyncModule : SyncModule<string>
    {
        protected override string? Execute(IModuleContext context, CancellationToken cancellationToken)
        {
            return "Hello from sync module";
        }
    }

    [Test]
    public async Task SyncModule_Executes_And_Returns_Value()
    {
        var module = await RunModule<SimpleSyncModule>();

        var result = await module;

        await Assert.That(result.ValueOrDefault).IsEqualTo("Hello from sync module");
        await Assert.That(result.ModuleResultType).IsEqualTo(ModuleResultType.Success);
    }

    public class SyncModuleReturningNull : SyncModule<string?>
    {
        protected override string? Execute(IModuleContext context, CancellationToken cancellationToken)
        {
            return null;
        }
    }

    [Test]
    public async Task SyncModule_Can_Return_Null()
    {
        var module = await RunModule<SyncModuleReturningNull>();

        var result = await module;

        await Assert.That(result.ValueOrDefault).IsNull();
        await Assert.That(result.ModuleResultType).IsEqualTo(ModuleResultType.Success);
    }

    public class SyncModuleWithComplexType : SyncModule<Dictionary<string, int>>
    {
        protected override Dictionary<string, int>? Execute(IModuleContext context, CancellationToken cancellationToken)
        {
            return new Dictionary<string, int>
            {
                ["one"] = 1,
                ["two"] = 2,
                ["three"] = 3,
            };
        }
    }

    [Test]
    public async Task SyncModule_Can_Return_Complex_Types()
    {
        var module = await RunModule<SyncModuleWithComplexType>();

        var result = await module;

        await Assert.That(result.ValueOrDefault).IsNotNull();
        await Assert.That(result.ValueOrDefault!.Count).IsEqualTo(3);
        await Assert.That(result.ValueOrDefault["two"]).IsEqualTo(2);
    }

    #endregion

    #region Exception Handling Tests

    public class ThrowingSyncModule : SyncModule<string>
    {
        protected override string? Execute(IModuleContext context, CancellationToken cancellationToken)
        {
            throw new InvalidOperationException("Sync module exception");
        }
    }

    [Test]
    public async Task SyncModule_Exception_Is_Captured()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<ThrowingSyncModule>()
            .BuildHostAsync();

        try
        {
            await host.ExecutePipelineAsync();
        }
        catch
        {
            // Expected
        }

        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();
        var result = resultRegistry.GetResult(typeof(ThrowingSyncModule))!;

        await Assert.That(result.ModuleStatus).IsEqualTo(Status.Failed);
        await Assert.That(result.ExceptionOrDefault).IsNotNull();
        await Assert.That(result.ExceptionOrDefault!.Message).IsEqualTo("Sync module exception");
    }

    #endregion

    #region Lifecycle Hook Tests

    public class SyncModuleWithBeforeHook : SyncModule<string>
    {
        public bool BeforeHookCalled { get; private set; }

        protected override void OnBeforeExecute(IModuleContext context, CancellationToken cancellationToken)
        {
            BeforeHookCalled = true;
        }

        protected override string? Execute(IModuleContext context, CancellationToken cancellationToken)
        {
            return "executed";
        }
    }

    [Test]
    public async Task SyncModule_OnBeforeExecute_Is_Called()
    {
        var module = await RunModule<SyncModuleWithBeforeHook>();

        await Assert.That(module.BeforeHookCalled).IsTrue();
    }

    public class SyncModuleWithAfterHook : SyncModule<string>
    {
        public bool AfterHookCalled { get; private set; }
        public ModuleResult<string>? CapturedResult { get; private set; }

        protected override ModuleResult<string>? OnAfterExecute(
            IModuleContext context,
            ModuleResult<string> result,
            CancellationToken cancellationToken)
        {
            AfterHookCalled = true;
            CapturedResult = result;
            return null;
        }

        protected override string? Execute(IModuleContext context, CancellationToken cancellationToken)
        {
            return "original";
        }
    }

    [Test]
    public async Task SyncModule_OnAfterExecute_Is_Called()
    {
        var module = await RunModule<SyncModuleWithAfterHook>();

        await Assert.That(module.AfterHookCalled).IsTrue();
        await Assert.That(module.CapturedResult).IsNotNull();
        await Assert.That(module.CapturedResult!.ValueOrDefault).IsEqualTo("original");
    }

    public class SyncModuleWithFailedHook : SyncModule<string>
    {
        public bool FailedHookCalled { get; private set; }
        public Exception? CapturedExceptionInHook { get; private set; }

        protected override void OnFailed(
            IModuleContext context,
            Exception exception,
            CancellationToken cancellationToken)
        {
            FailedHookCalled = true;
            CapturedExceptionInHook = exception;
        }

        protected override string? Execute(IModuleContext context, CancellationToken cancellationToken)
        {
            throw new InvalidOperationException("Test failure");
        }
    }

    [Test]
    public async Task SyncModule_OnFailed_Is_Called_On_Exception()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<SyncModuleWithFailedHook>()
            .BuildHostAsync();

        try
        {
            await host.ExecutePipelineAsync();
        }
        catch
        {
            // Expected
        }

        var modules = host.RootServices.GetServices<IModule>();
        var module = modules.OfType<SyncModuleWithFailedHook>().Single();

        await Assert.That(module.FailedHookCalled).IsTrue();
        await Assert.That(module.CapturedExceptionInHook).IsNotNull();
        await Assert.That(module.CapturedExceptionInHook!.Message).IsEqualTo("Test failure");
    }

    public class SyncModuleWithSkipConfig : SyncModule<string>
    {
        public bool SkippedHookCalled { get; private set; }
        public SkipDecision? CapturedSkipDecision { get; private set; }

        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithSkipWhen(() => SkipDecision.Skip("Always skip"))
            .Build();

        protected override void OnSkipped(
            IModuleContext context,
            SkipDecision skipDecision,
            CancellationToken cancellationToken)
        {
            SkippedHookCalled = true;
            CapturedSkipDecision = skipDecision;
        }

        protected override string? Execute(IModuleContext context, CancellationToken cancellationToken)
        {
            return "should not execute";
        }
    }

    [Test]
    public async Task SyncModule_OnSkipped_Is_Called_When_Skipped()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<SyncModuleWithSkipConfig>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var modules = host.RootServices.GetServices<IModule>();
        var module = modules.OfType<SyncModuleWithSkipConfig>().Single();

        await Assert.That(module.SkippedHookCalled).IsTrue();
        await Assert.That(module.CapturedSkipDecision).IsNotNull();
        await Assert.That(module.CapturedSkipDecision!.ShouldSkip).IsTrue();
        await Assert.That(module.CapturedSkipDecision.Reason).IsEqualTo("Always skip");
    }

    #endregion

    #region Dependency Tests

    public class SyncDependencyModule : SyncModule<int>
    {
        protected override int Execute(IModuleContext context, CancellationToken cancellationToken)
        {
            return 42;
        }
    }

    [ModularPipelines.Attributes.DependsOn<SyncDependencyModule>]
    public class SyncDependentModule : SyncModule<string>
    {
        protected override string? Execute(IModuleContext context, CancellationToken cancellationToken)
        {
            var dependency = context.GetModule<SyncDependencyModule, int>();
            return $"Dependency value: {dependency.ValueOrDefault}";
        }
    }

    [Test]
    public async Task SyncModule_Can_Depend_On_Another_SyncModule()
    {
        var (dependency, dependent) = await RunModules<SyncDependencyModule, SyncDependentModule>();

        var dependencyResult = await dependency;
        var dependentResult = await dependent;

        await Assert.That(dependencyResult.ValueOrDefault).IsEqualTo(42);
        await Assert.That(dependentResult.ValueOrDefault).IsEqualTo("Dependency value: 42");
    }

    public class AsyncDependencyModule : Module<int>
    {
        public override async Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return 100;
        }
    }

    [ModularPipelines.Attributes.DependsOn<AsyncDependencyModule>]
    public class SyncDependsOnAsync : SyncModule<string>
    {
        protected override string? Execute(IModuleContext context, CancellationToken cancellationToken)
        {
            var dependency = context.GetModule<AsyncDependencyModule, int>();
            return $"Async dependency value: {dependency.ValueOrDefault}";
        }
    }

    [Test]
    public async Task SyncModule_Can_Depend_On_AsyncModule()
    {
        var (asyncModule, syncModule) = await RunModules<AsyncDependencyModule, SyncDependsOnAsync>();

        var asyncResult = await asyncModule;
        var syncResult = await syncModule;

        await Assert.That(asyncResult.ValueOrDefault).IsEqualTo(100);
        await Assert.That(syncResult.ValueOrDefault).IsEqualTo("Async dependency value: 100");
    }

    public class SyncModuleForAsyncToDepend : SyncModule<int>
    {
        protected override int Execute(IModuleContext context, CancellationToken cancellationToken)
        {
            return 200;
        }
    }

    [ModularPipelines.Attributes.DependsOn<SyncModuleForAsyncToDepend>]
    public class AsyncDependsOnSync : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            var dependency = context.GetModule<SyncModuleForAsyncToDepend, int>();
            return $"Sync dependency value: {dependency.ValueOrDefault}";
        }
    }

    [Test]
    public async Task AsyncModule_Can_Depend_On_SyncModule()
    {
        var (syncModule, asyncModule) = await RunModules<SyncModuleForAsyncToDepend, AsyncDependsOnSync>();

        var syncResult = await syncModule;
        var asyncResult = await asyncModule;

        await Assert.That(syncResult.ValueOrDefault).IsEqualTo(200);
        await Assert.That(asyncResult.ValueOrDefault).IsEqualTo("Sync dependency value: 200");
    }

    #endregion

    #region Configuration Tests

    public class SyncModuleWithTimeout : SyncModule<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithTimeout(TimeSpan.FromMinutes(5))
            .Build();

        protected override string? Execute(IModuleContext context, CancellationToken cancellationToken)
        {
            return "configured";
        }
    }

    [Test]
    public async Task SyncModule_Respects_Configuration()
    {
        var module = await RunModule<SyncModuleWithTimeout>();

        var imodule = (IModule)module;
        await Assert.That(imodule.Configuration.Timeout).IsEqualTo(TimeSpan.FromMinutes(5));
    }

    public class SyncModuleWithRetry : SyncModule<string>
    {
        public int ExecutionCount { get; private set; }

        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithRetryCount(3)
            .Build();

        protected override string? Execute(IModuleContext context, CancellationToken cancellationToken)
        {
            ExecutionCount++;
            if (ExecutionCount < 3)
            {
                throw new InvalidOperationException($"Attempt {ExecutionCount} failed");
            }

            return "success on third try";
        }
    }

    [Test]
    public async Task SyncModule_Respects_Retry_Configuration()
    {
        var module = await RunModule<SyncModuleWithRetry>();

        var result = await module;

        await Assert.That(result.ValueOrDefault).IsEqualTo("success on third try");
        await Assert.That(module.ExecutionCount).IsEqualTo(3);
    }

    #endregion

    #region Cancellation Tests

    public class SyncModuleCheckingCancellation : SyncModule<string>
    {
        protected override string? Execute(IModuleContext context, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return "not cancelled";
        }
    }

    [Test]
    public async Task SyncModule_Receives_CancellationToken()
    {
        var module = await RunModule<SyncModuleCheckingCancellation>();

        var result = await module;

        await Assert.That(result.ValueOrDefault).IsEqualTo("not cancelled");
    }

    #endregion

    #region Context Access Tests

    public class SyncModuleAccessingContext : SyncModule<string>
    {
        protected override string? Execute(IModuleContext context, CancellationToken cancellationToken)
        {
            // Verify we can access context services
            var logger = context.Logger;
            logger.LogInformation("Logging from sync module");

            return "context accessed";
        }
    }

    [Test]
    public async Task SyncModule_Can_Access_Context()
    {
        var module = await RunModule<SyncModuleAccessingContext>();

        var result = await module;

        await Assert.That(result.ValueOrDefault).IsEqualTo("context accessed");
    }

    #endregion

    #region Test Helper Tests

    [Test]
    public async Task SimpleSyncTestModule_Helper_Works()
    {
        var module = await RunModule<SyncTrueModule>();

        var result = await module;

        await Assert.That(result.ValueOrDefault).IsTrue();
    }

    [Test]
    public async Task SyncNullModule_Helper_Works()
    {
        var module = await RunModule<SyncNullModule>();

        var result = await module;

        await Assert.That(result.ValueOrDefault).IsNull();
    }

    #endregion
}
