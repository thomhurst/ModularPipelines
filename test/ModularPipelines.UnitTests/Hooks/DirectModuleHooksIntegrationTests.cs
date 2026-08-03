using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes.Events;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Enums;
using ModularPipelines.Extensions;
using ModularPipelines.Interfaces;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Hooks;

/// <summary>
/// Integration tests for Direct Module-Level Hooks that test full pipeline execution scenarios.
/// </summary>
[NotInParallel(nameof(DirectModuleHooksIntegrationTests))]
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

        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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

    private sealed class RecordingModuleEventReceiver : IModuleEventReceiver
    {
        public Task OnModuleReadyAsync(IModuleHookContext context)
        {
            AddLogEntry("Global:Ready");
            return Task.CompletedTask;
        }

        public Task OnModuleStartAsync(IModuleHookContext context)
        {
            AddLogEntry("Global:Start");
            return Task.CompletedTask;
        }

        public Task OnModuleEndAsync(IModuleHookContext context)
        {
            AddLogEntry("Global:End");
            return Task.CompletedTask;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    private sealed class RecordingModuleEventAttribute : Attribute,
        IModuleReadyHandler,
        IModuleStartHandler,
        IModuleEndHandler
    {
        public Task OnModuleReadyAsync(IModuleHookContext context)
        {
            AddLogEntry("Attribute:Ready");
            return Task.CompletedTask;
        }

        public Task OnModuleStartAsync(IModuleHookContext context)
        {
            AddLogEntry("Attribute:Start");
            return Task.CompletedTask;
        }

        public Task OnModuleEndAsync(IModuleHookContext context, IModuleResult result)
        {
            AddLogEntry("Attribute:End");
            return Task.CompletedTask;
        }
    }

    [RecordingModuleEvent]
    private sealed class OrderedHooksModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            AddLogEntry("Module:Execute");
            return Task.FromResult<string>("Success");
        }

        protected override Task OnBeforeExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            AddLogEntry("Module:Before");
            return Task.CompletedTask;
        }

        protected override Task<ModuleResult<string>?> OnAfterExecuteAsync(
            IModuleContext context,
            ModuleResult<string> result,
            CancellationToken cancellationToken)
        {
            AddLogEntry("Module:After");
            return Task.FromResult<ModuleResult<string>?>(null);
        }
    }

    /// <summary>
    /// Module that depends on Module1 and uses hooks.
    /// </summary>
    [ModularPipelines.Attributes.DependsOn<Module1>]
    private class DependentLoggingModule : LoggingModule;

    /// <summary>
    /// Module that tracks context availability in hooks.
    /// </summary>
    private class ContextVerifyingModule : Module<string>
    {
        public bool ContextWasAvailableInBeforeHook { get; private set; }
        public bool ContextWasAvailableInAfterHook { get; private set; }
        public bool LoggerWasAvailableInBeforeHook { get; private set; }
        public bool LoggerWasAvailableInAfterHook { get; private set; }

        protected internal override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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
        var host = await TestPipelineBuilder.Create()
            .AddModule<Module1>()
            .AddModule<Module2>()
            .BuildAsync();

        await host.RunAsync();

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
    public async Task Global_Attribute_And_Module_Hooks_Have_Documented_Order()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<OrderedHooksModule>()
            .AddModuleEventReceiver<RecordingModuleEventReceiver>()
            .BuildAsync();

        await host.RunAsync();

        var log = GetLogSnapshot();
        var expected = new[]
        {
            "Global:Ready",
            "Global:Start",
            "Attribute:Ready",
            "Attribute:Start",
            "Module:Before",
            "Module:Execute",
            "Module:After",
            "Global:End",
            "Attribute:End",
        };

        await Assert.That(log).Count().IsEqualTo(expected.Length);
        for (var index = 0; index < expected.Length; index++)
        {
            await Assert.That(log[index]).IsEqualTo(expected[index]);
        }
    }

    [Test]
    public async Task DependentModule_HooksExecuteWithCorrectDependencyOrder()
    {
        var host = await TestPipelineBuilder.Create()
            .AddModule<Module1>()
            .AddModule<DependentLoggingModule>()
            .BuildAsync();

        await host.RunAsync();

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
        var host = await TestPipelineBuilder.Create()
            .AddModule<Module1>()
            .AddModule<Module2>()
            .BuildAsync();

        var result = await host.RunAsync();

        // Pipeline should complete successfully
        await Assert.That(result.Modules).Count().IsEqualTo(2);

        // All modules should have succeeded
        var resultRegistry = host.Services.GetRequiredService<IModuleResultRegistry>();

        var module1Result = resultRegistry.GetResult(typeof(Module1));
        var module2Result = resultRegistry.GetResult(typeof(Module2));

        await Assert.That(module1Result!.ModuleStatus).IsEqualTo(Status.Successful);
        await Assert.That(module2Result!.ModuleStatus).IsEqualTo(Status.Successful);
    }
}
