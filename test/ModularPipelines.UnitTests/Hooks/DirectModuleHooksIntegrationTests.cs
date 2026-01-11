using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Hooks;

/// <summary>
/// Integration tests for Direct Module-Level Hooks that test full pipeline execution scenarios.
/// </summary>
public class DirectModuleHooksIntegrationTests : TestBase
{
    #region Test Modules for Integration Tests

    /// <summary>
    /// A static log to track hook execution order across multiple modules.
    /// </summary>
    private static readonly List<string> ExecutionLog = [];
    private static readonly object LogLock = new();

    private static List<string> GetLogSnapshot()
    {
        lock (LogLock)
        {
            return ExecutionLog.ToList();
        }
    }

    private static void AddLogEntry(string entry)
    {
        lock (LogLock)
        {
            ExecutionLog.Add(entry);
        }
    }

    private static void ClearLog()
    {
        lock (LogLock)
        {
            ExecutionLog.Clear();
        }
    }

    /// <summary>
    /// Module that logs all lifecycle events for verification.
    /// </summary>
    private class LoggingModule : Module<string>
    {
        private readonly string _moduleId;

        public LoggingModule()
        {
            _moduleId = GetType().Name;
        }

        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            AddLogEntry($"{_moduleId}:ExecuteAsync");
            return $"{_moduleId} completed";
        }

        protected override Task OnBeforeExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            AddLogEntry($"{_moduleId}:OnBeforeExecuteAsync");
            return Task.CompletedTask;
        }

        protected override Task<ModuleResult<string>?> OnAfterExecuteAsync(
            IModuleContext context,
            ModuleResult<string> result,
            CancellationToken cancellationToken)
        {
            AddLogEntry($"{_moduleId}:OnAfterExecuteAsync");
            return Task.FromResult<ModuleResult<string>?>(null);
        }
    }

    private class Module1 : LoggingModule;
    private class Module2 : LoggingModule;

    /// <summary>
    /// Module that depends on Module1 and uses hooks.
    /// </summary>
    [ModularPipelines.Attributes.DependsOn<Module1>]
    private class DependentLoggingModule : LoggingModule;

    /// <summary>
    /// Module with multiple hook systems for ordering verification.
    /// </summary>
    private class MultiHookSystemModule : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithBeforeExecute(_ =>
            {
                AddLogEntry("MultiHook:Config:OnBeforeExecute");
                return Task.CompletedTask;
            })
            .WithAfterExecute(_ =>
            {
                AddLogEntry("MultiHook:Config:OnAfterExecute");
                return Task.CompletedTask;
            })
            .Build();

        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            AddLogEntry("MultiHook:ExecuteAsync");
            return "Success";
        }

        // Direct hook (should run first)
        protected override Task OnBeforeExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            AddLogEntry("MultiHook:Direct:OnBeforeExecuteAsync");
            return Task.CompletedTask;
        }

        protected override Task<ModuleResult<string>?> OnAfterExecuteAsync(
            IModuleContext context,
            ModuleResult<string> result,
            CancellationToken cancellationToken)
        {
            AddLogEntry("MultiHook:Direct:OnAfterExecuteAsync");
            return Task.FromResult<ModuleResult<string>?>(null);
        }
    }

    /// <summary>
    /// Module that tracks context availability in hooks.
    /// </summary>
    private class ContextVerifyingModule : Module<string>
    {
        public bool ContextWasAvailableInBeforeHook { get; private set; }
        public bool ContextWasAvailableInAfterHook { get; private set; }
        public bool LoggerWasAvailableInBeforeHook { get; private set; }
        public bool LoggerWasAvailableInAfterHook { get; private set; }

        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return "Success";
        }

        protected override Task OnBeforeExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            ContextWasAvailableInBeforeHook = context != null;
            LoggerWasAvailableInBeforeHook = context?.Logger != null;
            return Task.CompletedTask;
        }

        protected override Task<ModuleResult<string>?> OnAfterExecuteAsync(
            IModuleContext context,
            ModuleResult<string> result,
            CancellationToken cancellationToken)
        {
            ContextWasAvailableInAfterHook = context != null;
            LoggerWasAvailableInAfterHook = context?.Logger != null;
            return Task.FromResult<ModuleResult<string>?>(null);
        }
    }

    #endregion

    [Before(Test)]
    public void SetupTest()
    {
        ClearLog();
    }

    [Test]
    public async Task FullPipeline_WithMultipleModules_HooksExecuteInCorrectOrder()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<Module1>()
            .AddModule<Module2>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var log = GetLogSnapshot();

        // Module1's hooks should be: Before -> Execute -> After
        var module1Events = log.Where(e => e.StartsWith("Module1:")).ToList();
        await Assert.That(module1Events).Contains("Module1:OnBeforeExecuteAsync");
        await Assert.That(module1Events).Contains("Module1:ExecuteAsync");
        await Assert.That(module1Events).Contains("Module1:OnAfterExecuteAsync");

        var module1BeforeIndex = module1Events.IndexOf("Module1:OnBeforeExecuteAsync");
        var module1ExecuteIndex = module1Events.IndexOf("Module1:ExecuteAsync");
        var module1AfterIndex = module1Events.IndexOf("Module1:OnAfterExecuteAsync");

        await Assert.That(module1BeforeIndex).IsLessThan(module1ExecuteIndex);
        await Assert.That(module1ExecuteIndex).IsLessThan(module1AfterIndex);
    }

    [Test]
    public async Task HookOrdering_DirectHooksRunBeforeConfigHooks()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<MultiHookSystemModule>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var log = GetLogSnapshot();

        var directBeforeIndex = log.IndexOf("MultiHook:Direct:OnBeforeExecuteAsync");
        var configBeforeIndex = log.IndexOf("MultiHook:Config:OnBeforeExecute");
        var executeIndex = log.IndexOf("MultiHook:ExecuteAsync");

        // Direct hook should come before Config hook
        await Assert.That(directBeforeIndex).IsLessThan(configBeforeIndex);

        // Both hooks should come before execute
        await Assert.That(configBeforeIndex).IsLessThan(executeIndex);
    }

    [Test]
    public async Task DependentModule_HooksExecuteWithCorrectDependencyOrder()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<Module1>()
            .AddModule<DependentLoggingModule>()
            .BuildHostAsync();

        await host.ExecutePipelineAsync();

        var log = GetLogSnapshot();

        // Module1 should complete (including After hook) before DependentLoggingModule starts
        var module1AfterIndex = log.IndexOf("Module1:OnAfterExecuteAsync");
        var dependentBeforeIndex = log.IndexOf("DependentLoggingModule:OnBeforeExecuteAsync");

        // Dependent module's Before hook should only run after Module1 completes
        await Assert.That(module1AfterIndex).IsLessThan(dependentBeforeIndex);
    }

    [Test]
    public async Task Context_IsAvailableInHooks()
    {
        var module = await RunModule<ContextVerifyingModule>();

        await Assert.That(module.ContextWasAvailableInBeforeHook).IsTrue();
        await Assert.That(module.ContextWasAvailableInAfterHook).IsTrue();
        await Assert.That(module.LoggerWasAvailableInBeforeHook).IsTrue();
        await Assert.That(module.LoggerWasAvailableInAfterHook).IsTrue();
    }

    [Test]
    public async Task Pipeline_CompletesSuccessfully_WithAllHooksExecuted()
    {
        var host = await TestPipelineHostBuilder.Create()
            .AddModule<Module1>()
            .AddModule<Module2>()
            .BuildHostAsync();

        var result = await host.ExecutePipelineAsync();

        // Pipeline should complete successfully
        await Assert.That(result.Modules).HasCount().EqualTo(2);

        // All modules should have succeeded
        var resultRegistry = host.RootServices.GetRequiredService<IModuleResultRegistry>();

        var module1Result = resultRegistry.GetResult(typeof(Module1));
        var module2Result = resultRegistry.GetResult(typeof(Module2));

        await Assert.That(module1Result!.ModuleResultType).IsEqualTo(ModuleResultType.Success);
        await Assert.That(module2Result!.ModuleResultType).IsEqualTo(ModuleResultType.Success);
    }
}
