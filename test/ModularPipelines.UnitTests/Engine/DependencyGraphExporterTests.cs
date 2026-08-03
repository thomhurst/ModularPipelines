using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;
using ModularPipelines.Attributes.Events;
using ModularPipelines.Conditions;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Engine;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Enums;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;
using ModularPipelines.Interfaces;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.UnitTests.Engine;

[TUnit.Core.NotInParallel]
public class DependencyGraphExporterTests
{
    private static int _executions;
    private static int _asyncSkipConditionEvaluations;
    private static bool _startupConditionEnabled;
    private static bool _startupDependencyEnabled;
    private static bool _startupConfigurationEnabled;
    private static int _planningActivations;
    private static int _planningDisposals;
    private static int _planningRegistrationEvents;
    private static int _directModuleActivations;

    private sealed class DependencyModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref _executions);
            return Task.FromResult<string?>("dependency");
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    private sealed class AddRegistrationDependencyAttribute(Type dependencyType)
        : Attribute, IModuleRegistrationEventReceiver
    {
        public Task OnRegistrationAsync(IModuleRegistrationContext context)
        {
            context.AddDependency(dependencyType);
            return Task.CompletedTask;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    private sealed class AddStartupDependencyAttribute(Type dependencyType)
        : Attribute, IModuleRegistrationEventReceiver
    {
        public Task OnRegistrationAsync(IModuleRegistrationContext context)
        {
            if (_startupDependencyEnabled)
            {
                context.AddDependency(dependencyType);
            }

            return Task.CompletedTask;
        }
    }

    private sealed class UnregisteredModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("unregistered");
    }

    [AddRegistrationDependency(typeof(UnregisteredModule))]
    private sealed class InvalidDynamicDependencyModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("invalid");
    }

    [AddRegistrationDependency(typeof(DependencyModule))]
    private sealed class DynamicDependencyModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("dynamic");
    }

    private sealed class HistoricalDependencyModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            throw new InvalidOperationException("History should satisfy this module.");
    }

    [ModularPipelines.Attributes.DependsOn<HistoricalDependencyModule>]
    private sealed class HistoricalDependentModule : Module<string>
    {
        protected internal override async Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            var dependency = await context.GetModule<HistoricalDependencyModule>();
            return dependency.ValueOrDefault;
        }
    }

    [AddRegistrationDependency(typeof(UnregisteredModule))]
    private sealed class InvalidSkippedDynamicDependencyModule : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithSkipWhen(_ => SkipDecision.Skip("configured skip"))
            .Build();

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("invalid-skipped");
    }

    [ModuleCategory("build\r\nrelease")]
    private sealed class LineBreakCategoryModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("line-break");
    }

    [ModuleCategory("build\r\n```\r\nrelease")]
    private sealed class MarkdownFenceCategoryModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("markdown-fence");
    }

    [ModularPipelines.Attributes.DependsOnAttribute<DependencyModule>]
    [ModuleCategory(@"build C:\new")]
    private sealed class TargetModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref _executions);
            return Task.FromResult<string?>("target");
        }
    }

    private sealed class SkippedModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref _executions);
            return Task.FromResult<string?>("skipped");
        }
    }

    private sealed class NeverRunCondition : IRunCondition
    {
        public Task<bool> EvaluateAsync(IPipelineContext context) => Task.FromResult(false);
    }

    private sealed class StartupStateCondition : IRunCondition
    {
        public Task<bool> EvaluateAsync(IPipelineContext context) =>
            Task.FromResult(_startupConditionEnabled);
    }

    private sealed class EnableStartupConditionHook : IPipelineGlobalHooks
    {
        public Task OnPipelineStartAsync(IPipelineContext context)
        {
            _startupConditionEnabled = true;
            return Task.CompletedTask;
        }
    }

    private sealed class EnableStartupDependencyHook : IPipelineGlobalHooks
    {
        public Task OnPipelineStartAsync(IPipelineContext context)
        {
            _startupDependencyEnabled = true;
            return Task.CompletedTask;
        }
    }

    private sealed class EnableStartupConfigurationHook : IPipelineGlobalHooks
    {
        public Task OnPipelineStartAsync(IPipelineContext context)
        {
            _startupConfigurationEnabled = true;
            return Task.CompletedTask;
        }
    }

    private sealed class PlanningCategoryModule : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithCategory("planning")
            .Build();

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("planning-category");
    }

    private sealed class MutableConfigurationStateModule : Module<string>
    {
        private readonly List<string> _configurationCalls = [];

        protected override ModuleConfiguration Configure()
        {
            _configurationCalls.Add("configured");
            if (_configurationCalls.Count > 1)
            {
                throw new InvalidOperationException("Mutable configuration state was shared.");
            }

            return ModuleConfiguration.Default;
        }

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("mutable-state");
    }

    private sealed class FactoryInitializedModule : Module<string>
    {
        public bool IncludeDependency { get; init; }

        protected override ModuleConfiguration Configure()
        {
            var builder = ModuleConfiguration.Create();
            if (IncludeDependency)
            {
                builder.DependsOn<DependencyModule>();
            }

            return builder.Build();
        }

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("factory-initialized");
    }

    private sealed class FactoryInitializedPlanningCopyModule : Module<string>
    {
        public bool IncludeDependency { get; init; }

        protected override ModuleConfiguration Configure()
        {
            var builder = ModuleConfiguration.Create();
            if (IncludeDependency)
            {
                builder.DependsOn<DependencyModule>();
            }

            return builder.Build();
        }

        protected override Module<string> CreatePlanningCopy(IServiceProvider serviceProvider) =>
            new FactoryInitializedPlanningCopyModule { IncludeDependency = IncludeDependency };

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("factory-initialized-planning-copy");
    }

    private sealed class SharedMutableFactoryStateModule(List<string> state) : Module<string>
    {
        protected override ModuleConfiguration Configure()
        {
            state.Add("configured");
            return ModuleConfiguration.Default;
        }

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("shared-mutable-state");
    }

    private readonly record struct StructWrappedState(List<string> Values);

    private sealed class StructWrappedFactoryStateModule(StructWrappedState state) : Module<string>
    {
        protected override ModuleConfiguration Configure()
        {
            state.Values.Add("configured");
            return ModuleConfiguration.Default;
        }

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("struct-wrapped-state");
    }

    private sealed class OverloadedPlanningCopyModule : Module<string>
    {
        private Module<string> CreatePlanningCopy() =>
            throw new InvalidOperationException("The parameterless helper must not be selected.");

        protected override Module<string> CreatePlanningCopy(IServiceProvider serviceProvider) =>
            new OverloadedPlanningCopyModule();

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("overloaded-planning-copy");
    }

    private sealed class FieldlessStateFactoryModule : Module<string>
    {
        private readonly object _gate = new();

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            lock (_gate)
            {
                return Task.FromResult<string?>("fieldless-state");
            }
        }
    }

    private sealed class DirectInterfaceModule : IModule
    {
        public DirectInterfaceModule()
        {
            Interlocked.Increment(ref _directModuleActivations);
        }

        public Type ResultType => typeof(string);

        public ModuleConfiguration Configuration => ModuleConfiguration.Default;

        public Task<IModuleResult> ResultTask =>
            Task.FromException<IModuleResult>(new InvalidOperationException("Not executed by this test."));

        public bool TrySetDistributedResult(IModuleResult result) => false;
    }

    private sealed class PlanningSingletonDependency
    {
        public bool IncludeDependency { get; init; }
    }

    private sealed class ServiceBackedFactoryModule(PlanningSingletonDependency dependency)
        : Module<string>
    {
        protected override ModuleConfiguration Configure()
        {
            var builder = ModuleConfiguration.Create();
            if (dependency.IncludeDependency)
            {
                builder.DependsOn<DependencyModule>();
            }

            return builder.Build();
        }

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("service-backed");
    }

    private class FactorySettingsBase
    {
        public bool IncludeDependency { get; init; }
    }

    private sealed class DerivedFactorySettings : FactorySettingsBase
    {
        public string Marker { get; init; } = "same";
    }

    private sealed class InheritedSettingsFactoryModule(DerivedFactorySettings settings)
        : Module<string>
    {
        protected override ModuleConfiguration Configure()
        {
            var builder = ModuleConfiguration.Create();
            if (settings.IncludeDependency)
            {
                builder.DependsOn<DependencyModule>();
            }

            return builder.Build();
        }

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("inherited-settings");
    }

    private sealed class AliasState
    {
        public bool IncludeDependency { get; init; }
    }

    private sealed class AliasTopologyFactoryModule(AliasState first, AliasState second)
        : Module<string>
    {
        protected override ModuleConfiguration Configure()
        {
            var builder = ModuleConfiguration.Create();
            if (ReferenceEquals(first, second) && first.IncludeDependency)
            {
                builder.DependsOn<DependencyModule>();
            }

            return builder.Build();
        }

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("alias-topology");
    }

    private sealed class ArrayShapeFactoryModule(Array state) : Module<string>
    {
        protected override ModuleConfiguration Configure()
        {
            var builder = ModuleConfiguration.Create();
            if (state.Rank == 2
                && state.GetLength(0) == 1
                && state.GetLowerBound(0) == 0)
            {
                builder.DependsOn<DependencyModule>();
            }

            return builder.Build();
        }

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("array-shape");
    }

    private sealed class ComparerBackedFactoryModule(HashSet<string> values) : Module<string>
    {
        protected override ModuleConfiguration Configure()
        {
            var builder = ModuleConfiguration.Create();
            if (values.Comparer.Equals("dependency", "DEPENDENCY"))
            {
                builder.DependsOn<DependencyModule>();
            }

            return builder.Build();
        }

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("comparer-backed");
    }

    private sealed class PrecreatedModuleSettings
    {
        public bool IncludeDependency { get; init; }
    }

    private sealed class PrecreatedConfiguredModule(PrecreatedModuleSettings settings) : Module<string>
    {
        protected override ModuleConfiguration Configure()
        {
            var builder = ModuleConfiguration.Create();
            if (settings.IncludeDependency)
            {
                builder.DependsOn<DependencyModule>();
            }

            return builder.Build();
        }

        protected override Module<string> CreatePlanningCopy(IServiceProvider serviceProvider) =>
            new PrecreatedConfiguredModule(new PrecreatedModuleSettings
            {
                IncludeDependency = settings.IncludeDependency,
            });

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("precreated");
    }

    private sealed class PrecreatedDisposableState : IDisposable
    {
        public bool IsDisposed { get; private set; }

        public void Dispose() => IsDisposed = true;
    }

    private sealed class PrecreatedDisposableModule(PrecreatedDisposableState state)
        : Module<string>, IDisposable
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            ObjectDisposedException.ThrowIf(state.IsDisposed, state);
            return Task.FromResult<string?>("precreated-disposable");
        }

        public void Dispose() => state.Dispose();
    }

    private sealed class CapturingComparerFactoryModule(IComparer<string> comparer) : Module<string>
    {
        protected override ModuleConfiguration Configure()
        {
            var builder = ModuleConfiguration.Create();
            if (comparer.Compare("dependency", "other") < 0)
            {
                builder.DependsOn<DependencyModule>();
            }

            return builder.Build();
        }

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("capturing-comparer");
    }

    private class FactoryInitializedBaseModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("factory-initialized-base");
    }

    private sealed class FactoryInitializedDerivedModule : FactoryInitializedBaseModule
    {
        public bool IncludeDependency { get; init; }

        protected override ModuleConfiguration Configure()
        {
            var builder = ModuleConfiguration.Create();
            if (IncludeDependency)
            {
                builder.DependsOn<DependencyModule>();
            }

            return builder.Build();
        }

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("factory-initialized-derived");
    }

    private sealed class ContainerOwnedPlanningModule : Module<string>, IAsyncDisposable
    {
        public ValueTask DisposeAsync()
        {
            Interlocked.Increment(ref _planningDisposals);
            return ValueTask.CompletedTask;
        }

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("container-owned-planning");
    }

    private sealed class ContainerOwnedPlanningModuleFactory(IServiceProvider serviceProvider)
    {
        public ContainerOwnedPlanningModule Create() =>
            serviceProvider.GetRequiredService<ContainerOwnedPlanningModule>();
    }

    private sealed class ContainerOwnedPlanningState;

    private sealed class ContainerOwnedPlanningStateFactory(IServiceProvider serviceProvider)
    {
        public ContainerOwnedPlanningState Create() =>
            serviceProvider.GetRequiredService<ContainerOwnedPlanningState>();
    }

    private sealed class ModuleWithContainerOwnedPlanningState(
        ContainerOwnedPlanningState planningState) : Module<string>
    {
        private readonly ContainerOwnedPlanningState _planningState = planningState;

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>(_planningState.GetType().Name);
    }

    private sealed class PlanningFactoryDependency;

    private sealed class ContainerOwnedPlanningCopyModule : Module<string>, IAsyncDisposable
    {
        protected override Module<string> CreatePlanningCopy(IServiceProvider serviceProvider) =>
            serviceProvider.GetRequiredKeyedService<ContainerOwnedPlanningCopyModule>("planning");

        public ValueTask DisposeAsync()
        {
            Interlocked.Increment(ref _planningDisposals);
            return ValueTask.CompletedTask;
        }

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("container-owned-planning-copy");
    }

    [CountPlanningRegistration]
    private sealed class DisposablePlanningModule : Module<string>, IAsyncDisposable
    {
        public DisposablePlanningModule()
        {
            Interlocked.Increment(ref _planningActivations);
        }

        public ValueTask DisposeAsync()
        {
            Interlocked.Increment(ref _planningDisposals);
            return ValueTask.CompletedTask;
        }

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("disposable-planning");
    }

    private sealed class CountPlanningRegistrationAttribute
        : Attribute, IModuleRegistrationEventReceiver
    {
        public Task OnRegistrationAsync(IModuleRegistrationContext context)
        {
            Interlocked.Increment(ref _planningRegistrationEvents);
            return Task.CompletedTask;
        }
    }

    private sealed class SingletonFactoryModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("singleton-factory");
    }

    private sealed class StartupConfiguredModule : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithSkipWhen(_ => _startupConfigurationEnabled
                ? SkipDecision.DoNotSkip
                : SkipDecision.Skip("startup configuration is not ready"))
            .Build();

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref _executions);
            return Task.FromResult<string?>("startup-configured");
        }
    }

    private sealed class FactorySkipModule(bool shouldSkip) : Module<string>
    {
        protected override ModuleConfiguration Configure()
        {
            var capturedDecision = shouldSkip
                ? SkipDecision.Skip("factory requested a skip")
                : SkipDecision.DoNotSkip;
            return ModuleConfiguration.Create()
                .WithSkipWhen(_ => capturedDecision)
                .Build();
        }

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("factory-skip");
    }

    [AttributeUsage(AttributeTargets.Class)]
    private sealed class SingleUseConditionAttribute : RunIfAllAttribute
    {
        private bool _evaluated;

        public override Task<bool> EvaluateAsync(IPipelineContext context)
        {
            if (_evaluated)
            {
                throw new InvalidOperationException("Condition attribute was reused.");
            }

            _evaluated = true;
            return Task.FromResult(true);
        }
    }

    [SingleUseCondition]
    private sealed class SingleUseConditionModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("single-use");
    }

    [AddStartupDependency(typeof(DependencyModule))]
    private sealed class StartupDynamicDependencyModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("startup-dynamic");
    }

    [RunIfAll<StartupStateCondition>]
    private sealed class StartupConditionModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref _executions);
            return Task.FromResult<string?>("startup-condition");
        }
    }

    [RunIfAll<NeverRunCondition>]
    private sealed class ConditionSkippedModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref _executions);
            return Task.FromResult<string?>("condition-skipped");
        }
    }

    [ModularPipelines.Attributes.DependsOn<ConditionSkippedModule>]
    private sealed class DependentOnConditionSkippedModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            Interlocked.Increment(ref _executions);
            return Task.FromResult<string?>("dependent");
        }
    }

    [ModularPipelines.Attributes.DependsOn<DependentOnConditionSkippedModule>]
    private sealed class DownstreamOfConditionSkippedModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("downstream");
    }

    private sealed class ConfiguredSkippedModule : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithSkipWhen(_ => SkipDecision.Skip("configured skip"))
            .Build();

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("configured-skipped");
    }

    [ModularPipelines.Attributes.DependsOn<ConfiguredSkippedModule>]
    private sealed class DependentOnConfiguredSkippedModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("dependent");
    }

    [ModularPipelines.Attributes.DependsOn<DependencyModule>]
    private sealed class ResultDependentConfiguredSkipModule : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithSkipWhen(async (context, _) =>
            {
                await context.GetModule<DependencyModule>();
                return SkipDecision.DoNotSkip;
            })
            .Build();

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("result-dependent");
    }

    private sealed class AsyncConfiguredSkipModule : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithSkipWhen(async (_, cancellationToken) =>
            {
                Interlocked.Increment(ref _asyncSkipConditionEvaluations);
                await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
                return SkipDecision.DoNotSkip;
            })
            .Build();

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("async-configured");
    }

    private sealed class SynchronouslySkippedBeforeAsyncModule : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithSkipWhen(_ => SkipDecision.Skip("synchronous short circuit"))
            .WithSkipWhen(async (_, _) =>
            {
                Interlocked.Increment(ref _asyncSkipConditionEvaluations);
                await Task.Yield();
                return SkipDecision.DoNotSkip;
            })
            .Build();

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("mixed-configured");
    }

    private sealed class SynchronouslySkippedAfterAsyncModule : Module<string>
    {
        protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
            .WithSkipWhen(async (_, _) =>
            {
                Interlocked.Increment(ref _asyncSkipConditionEvaluations);
                await Task.Yield();
                return SkipDecision.DoNotSkip;
            })
            .WithSkipWhen(_ => SkipDecision.Skip("synchronous short circuit"))
            .Build();

        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("mixed-configured");
    }

    [ModularPipelines.Attributes.DependsOn<SynchronouslySkippedBeforeAsyncModule>]
    private sealed class DependentOnSynchronouslySkippedBeforeAsyncModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("dependent");
    }

    [ModularPipelines.Attributes.DependsOn<ResultDependentConfiguredSkipModule>]
    private sealed class DependentOnUnresolvedSkipModule : Module<string>
    {
        protected internal override Task<string?> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken) =>
            Task.FromResult<string?>("dependent");
    }

    private sealed class FixedEstimatedTimeProvider : IModuleEstimatedTimeProvider
    {
        public Task<TimeSpan> GetModuleEstimatedTimeAsync(Type moduleType) =>
            Task.FromResult(TimeSpan.FromSeconds(5));

        public Task SaveModuleTimeAsync(Type moduleType, TimeSpan duration) =>
            Task.CompletedTask;

        public Task<IEnumerable<SubModuleEstimation>> GetSubModuleEstimatedTimesAsync(Type moduleType) =>
            Task.FromResult<IEnumerable<SubModuleEstimation>>([]);

        public Task SaveSubModuleTimeAsync(
            Type moduleType,
            SubModuleEstimation subModuleEstimation) =>
            Task.CompletedTask;
    }

    private sealed class CancelingEstimatedTimeProvider(CancellationTokenSource cancellationTokenSource)
        : IModuleEstimatedTimeProvider
    {
        public Task<TimeSpan> GetModuleEstimatedTimeAsync(Type moduleType)
        {
            cancellationTokenSource.Cancel();
            return Task.FromResult(TimeSpan.FromSeconds(5));
        }

        public Task SaveModuleTimeAsync(Type moduleType, TimeSpan duration) =>
            Task.CompletedTask;

        public Task<IEnumerable<SubModuleEstimation>> GetSubModuleEstimatedTimesAsync(Type moduleType) =>
            Task.FromResult<IEnumerable<SubModuleEstimation>>([]);

        public Task SaveSubModuleTimeAsync(
            Type moduleType,
            SubModuleEstimation subModuleEstimation) =>
            Task.CompletedTask;
    }

    private sealed class DependencyHistoryRepository : IModuleResultRepository
    {
        public bool IsEnabled => true;

        public Task SaveResultAsync<T>(
            Module<T> module,
            ModuleResult<T> moduleResult,
            IPipelineContext pipelineContext) =>
            Task.CompletedTask;

        public Task<ModuleResult<T>?> GetResultAsync<T>(
            Module<T> module,
            IPipelineContext pipelineContext)
        {
            var executionContext = new ModuleExecutionContext(module, module.GetType());
            return Task.FromResult<ModuleResult<T>?>(
                ModuleResult<T>.CreateSuccess(default!, executionContext));
        }
    }

    private sealed class ModuleTypeHistoryRepository(Type moduleType) : IModuleResultRepository
    {
        public bool IsEnabled => true;

        public Task SaveResultAsync<T>(
            Module<T> module,
            ModuleResult<T> moduleResult,
            IPipelineContext pipelineContext) =>
            Task.CompletedTask;

        public Task<ModuleResult<T>?> GetResultAsync<T>(
            Module<T> module,
            IPipelineContext pipelineContext)
        {
            if (module.GetType() != moduleType)
            {
                return Task.FromResult<ModuleResult<T>?>(null);
            }

            var executionContext = new ModuleExecutionContext(module, module.GetType());
            return Task.FromResult<ModuleResult<T>?>(
                ModuleResult<T>.CreateSuccess(default!, executionContext));
        }
    }

    private sealed class CancelingModuleTypeHistoryRepository(
        Type moduleType,
        CancellationTokenSource cancellationTokenSource) : IModuleResultRepository
    {
        public bool IsEnabled => true;

        public Task SaveResultAsync<T>(
            Module<T> module,
            ModuleResult<T> moduleResult,
            IPipelineContext pipelineContext) =>
            Task.CompletedTask;

        public Task<ModuleResult<T>?> GetResultAsync<T>(
            Module<T> module,
            IPipelineContext pipelineContext)
        {
            if (module.GetType() != moduleType)
            {
                return Task.FromResult<ModuleResult<T>?>(null);
            }

            cancellationTokenSource.Cancel();
            var executionContext = new ModuleExecutionContext(module, module.GetType());
            return Task.FromResult<ModuleResult<T>?>(
                ModuleResult<T>.CreateSuccess(default!, executionContext));
        }
    }

    private sealed class ChangingHistoryRepository : IModuleResultRepository
    {
        private int _readCount;

        public bool IsEnabled => true;

        public int ReadCount => Volatile.Read(ref _readCount);

        public Task SaveResultAsync<T>(
            Module<T> module,
            ModuleResult<T> moduleResult,
            IPipelineContext pipelineContext) =>
            Task.CompletedTask;

        public Task<ModuleResult<T>?> GetResultAsync<T>(
            Module<T> module,
            IPipelineContext pipelineContext)
        {
            var value = $"history-{Interlocked.Increment(ref _readCount)}";
            var executionContext = new ModuleExecutionContext(module, module.GetType());
            return Task.FromResult<ModuleResult<T>?>(
                ModuleResult<T>.CreateSuccess((T) (object) value, executionContext));
        }
    }

    private sealed class InstanceBoundHistoryRepository(IModule expectedModule)
        : IModuleResultRepository
    {
        public bool IsEnabled => true;

        public bool ReceivedExpectedModule { get; private set; }

        public Task SaveResultAsync<T>(
            Module<T> module,
            ModuleResult<T> moduleResult,
            IPipelineContext pipelineContext) =>
            Task.CompletedTask;

        public Task<ModuleResult<T>?> GetResultAsync<T>(
            Module<T> module,
            IPipelineContext pipelineContext)
        {
            ReceivedExpectedModule = ReferenceEquals(module, expectedModule);
            if (!ReceivedExpectedModule)
            {
                return Task.FromResult<ModuleResult<T>?>(null);
            }

            var executionContext = new ModuleExecutionContext(module, module.GetType());
            return Task.FromResult<ModuleResult<T>?>(
                ModuleResult<T>.CreateSuccess(default!, executionContext));
        }
    }

    [Before(Test)]
    public void ResetExecutions()
    {
        _executions = 0;
        _asyncSkipConditionEvaluations = 0;
        _startupConditionEnabled = false;
        _startupDependencyEnabled = false;
        _startupConfigurationEnabled = false;
        _planningActivations = 0;
        _planningDisposals = 0;
        _planningRegistrationEvents = 0;
    }

    [Test]
    public async Task Renderers_Describe_The_Same_Annotated_Graph()
    {
        using var builder = CreateBuilder();
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        var mermaid = await exporter.RenderAsync(DependencyGraphFormat.Mermaid);
        var dot = await exporter.RenderAsync(DependencyGraphFormat.Dot);
        var json = await exporter.RenderAsync(DependencyGraphFormat.Json);

        using var document = JsonDocument.Parse(json);
        var nodes = document.RootElement.GetProperty("nodes");
        var edges = document.RootElement.GetProperty("edges");
        var targetNode = nodes.EnumerateArray()
            .Single(node => node.GetProperty("name").GetString() == nameof(TargetModule));
        using (Assert.Multiple())
        {
            await Assert.That(mermaid).Contains("flowchart TD");
            await Assert.That(mermaid).Contains("Category: build");
            await Assert.That(mermaid).Contains("Estimated: 00:00:05");
            await Assert.That(mermaid).Contains("Skipped:");
            await Assert.That(dot).Contains("digraph ModularPipelines");
            await Assert.That(dot).Contains(@"Category: build C:\\new");
            await Assert.That(dot).Contains("n0 -> n2");
            await Assert.That(nodes.GetArrayLength()).IsEqualTo(3);
            await Assert.That(edges.GetArrayLength()).IsEqualTo(1);
            await Assert.That(targetNode.GetProperty("skipped").GetBoolean()).IsTrue();
            await Assert.That(targetNode.GetProperty("skipReason").GetString())
                .Contains(nameof(DependencyModule));
            await Assert.That(_executions).IsEqualTo(0);
        }
    }

    [Test]
    public async Task Builder_Exports_Graph_Without_Executing_Modules()
    {
        var directory = Directory.CreateTempSubdirectory("modular-pipelines-graph-");
        try
        {
            var path = Path.Combine(directory.FullName, "graph.json");
            using var builder = CreateBuilder();

            await builder.ExportDependencyGraphAsync(
                DependencyGraphFormat.Json,
                path);

            using var document = JsonDocument.Parse(await File.ReadAllTextAsync(path));
            await Assert.That(document.RootElement.GetProperty("nodes").GetArrayLength())
                .IsEqualTo(3);
            await Assert.That(_executions).IsEqualTo(0);
        }
        finally
        {
            directory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Historical_Ignored_Dependency_Does_Not_Skip_Dependent()
    {
        using var builder = CreateBuilder();
        builder.Services.AddSingleton<IModuleResultRepository>(new DependencyHistoryRepository());
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        using var document = JsonDocument.Parse(
            await exporter.RenderAsync(DependencyGraphFormat.Json));
        var nodes = document.RootElement.GetProperty("nodes").EnumerateArray().ToArray();
        var dependencyNode = nodes.Single(node =>
            node.GetProperty("name").GetString() == nameof(DependencyModule));
        var targetNode = nodes.Single(node =>
            node.GetProperty("name").GetString() == nameof(TargetModule));

        using (Assert.Multiple())
        {
            await Assert.That(dependencyNode.GetProperty("skipped").GetBoolean()).IsFalse();
            await Assert.That(dependencyNode.GetProperty("skipReason").ValueKind)
                .IsEqualTo(JsonValueKind.Null);
            await Assert.That(targetNode.GetProperty("skipped").GetBoolean()).IsFalse();
            await Assert.That(_executions).IsEqualTo(0);
        }
    }

    [Test]
    public async Task Render_Does_Not_Complete_Runtime_Module_Results()
    {
        var repository = new ChangingHistoryRepository();
        using var builder = Pipeline.CreateBuilder();
        builder.Services.AddSingleton<IModuleResultRepository>(repository);
        builder.ConfigurePipelineOptions(options => options with
        {
            SkippedModules = [nameof(HistoricalDependencyModule)],
        });
        builder.AddModule<HistoricalDependencyModule>();
        builder.AddModule<HistoricalDependentModule>();
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        _ = await exporter.RenderAsync(DependencyGraphFormat.Json);
        var summary = await pipeline.RunAsync();
        var dependentResult = summary.Results.Single(result =>
            result.ModuleName == nameof(HistoricalDependentModule));

        using (Assert.Multiple())
        {
            await Assert.That(repository.ReadCount).IsEqualTo(2);
            await Assert.That(dependentResult.ValueOrDefault).IsEqualTo("history-2");
        }
    }

    [Test]
    public async Task Render_Uses_Registered_Module_Instance_For_History()
    {
        var dependency = new HistoricalDependencyModule();
        var repository = new InstanceBoundHistoryRepository(dependency);
        using var builder = Pipeline.CreateBuilder();
        builder.Services.AddSingleton<IModuleResultRepository>(repository);
        builder.ConfigurePipelineOptions(options => options with
        {
            SkippedModules = [nameof(HistoricalDependencyModule)],
        });
        builder.AddModule(dependency);
        builder.AddModule<HistoricalDependentModule>();
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        using var document = JsonDocument.Parse(
            await exporter.RenderAsync(DependencyGraphFormat.Json));
        var nodes = document.RootElement.GetProperty("nodes").EnumerateArray().ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(repository.ReceivedExpectedModule).IsTrue();
            await Assert.That(nodes.All(node => !node.GetProperty("skipped").GetBoolean()))
                .IsTrue();
        }
    }

    [Test]
    public async Task Configured_Skip_With_History_Does_Not_Skip_Dependent()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.Services.AddSingleton<IModuleResultRepository>(new DependencyHistoryRepository());
        builder.AddModule<ConfiguredSkippedModule>();
        builder.AddModule<DependentOnConfiguredSkippedModule>();
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        using var document = JsonDocument.Parse(
            await exporter.RenderAsync(DependencyGraphFormat.Json));
        var nodes = document.RootElement.GetProperty("nodes").EnumerateArray().ToArray();
        var configuredNode = nodes.Single(node =>
            node.GetProperty("name").GetString() == nameof(ConfiguredSkippedModule));
        var dependentNode = nodes.Single(node =>
            node.GetProperty("name").GetString() == nameof(DependentOnConfiguredSkippedModule));

        using (Assert.Multiple())
        {
            await Assert.That(configuredNode.GetProperty("skipped").GetBoolean()).IsFalse();
            await Assert.That(configuredNode.GetProperty("skipReason").ValueKind)
                .IsEqualTo(JsonValueKind.Null);
            await Assert.That(dependentNode.GetProperty("skipped").GetBoolean()).IsFalse();
        }
    }

    [Test]
    public async Task Run_Condition_With_History_Does_Not_Skip_Dependent()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.Services.AddSingleton<IModuleResultRepository>(new DependencyHistoryRepository());
        builder.AddModule<ConditionSkippedModule>();
        builder.AddModule<DependentOnConditionSkippedModule>();
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        using var document = JsonDocument.Parse(
            await exporter.RenderAsync(DependencyGraphFormat.Json));
        var nodes = document.RootElement.GetProperty("nodes").EnumerateArray().ToArray();
        var conditionNode = nodes.Single(node =>
            node.GetProperty("name").GetString() == nameof(ConditionSkippedModule));
        var dependentNode = nodes.Single(node =>
            node.GetProperty("name").GetString() == nameof(DependentOnConditionSkippedModule));

        using (Assert.Multiple())
        {
            await Assert.That(conditionNode.GetProperty("skipped").GetBoolean()).IsFalse();
            await Assert.That(conditionNode.GetProperty("skipReason").ValueKind)
                .IsEqualTo(JsonValueKind.Null);
            await Assert.That(dependentNode.GetProperty("skipped").GetBoolean()).IsFalse();
        }
    }

    [Test]
    public async Task Cascaded_Dependency_With_History_Does_Not_Skip_Downstream_Module()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.Services.AddSingleton<IModuleResultRepository>(
            new ModuleTypeHistoryRepository(typeof(DependentOnConditionSkippedModule)));
        builder.AddModule<ConditionSkippedModule>();
        builder.AddModule<DependentOnConditionSkippedModule>();
        builder.AddModule<DownstreamOfConditionSkippedModule>();
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        using var document = JsonDocument.Parse(
            await exporter.RenderAsync(DependencyGraphFormat.Json));
        var nodes = document.RootElement.GetProperty("nodes").EnumerateArray().ToArray();
        var conditionNode = nodes.Single(node =>
            node.GetProperty("name").GetString() == nameof(ConditionSkippedModule));
        var dependentNode = nodes.Single(node =>
            node.GetProperty("name").GetString() == nameof(DependentOnConditionSkippedModule));
        var downstreamNode = nodes.Single(node =>
            node.GetProperty("name").GetString() == nameof(DownstreamOfConditionSkippedModule));

        using (Assert.Multiple())
        {
            await Assert.That(conditionNode.GetProperty("skipped").GetBoolean()).IsTrue();
            await Assert.That(dependentNode.GetProperty("skipped").GetBoolean()).IsFalse();
            await Assert.That(downstreamNode.GetProperty("skipped").GetBoolean()).IsFalse();
        }
    }

    [Test]
    public async Task Cascaded_History_Lookup_Observes_Cancellation()
    {
        using var cancellationTokenSource = new CancellationTokenSource();
        using var builder = Pipeline.CreateBuilder();
        builder.Services.AddSingleton<IModuleResultRepository>(
            new CancelingModuleTypeHistoryRepository(
                typeof(DependentOnConditionSkippedModule),
                cancellationTokenSource));
        builder.AddModule<ConditionSkippedModule>();
        builder.AddModule<DependentOnConditionSkippedModule>();
        builder.AddModule<DownstreamOfConditionSkippedModule>();
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        await Assert.ThrowsAsync<OperationCanceledException>(
            () => exporter.RenderAsync(
                DependencyGraphFormat.Json,
                cancellationTokenSource.Token));
    }

    [Test]
    public async Task Run_Conditions_And_Their_Cascade_Are_Annotated_As_Skipped()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule<ConditionSkippedModule>();
        builder.AddModule<DependentOnConditionSkippedModule>();
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        using var document = JsonDocument.Parse(
            await exporter.RenderAsync(DependencyGraphFormat.Json));
        var nodes = document.RootElement.GetProperty("nodes").EnumerateArray().ToArray();
        var conditionNode = nodes.Single(node =>
            node.GetProperty("name").GetString() == nameof(ConditionSkippedModule));
        var dependentNode = nodes.Single(node =>
            node.GetProperty("name").GetString() == nameof(DependentOnConditionSkippedModule));

        using (Assert.Multiple())
        {
            await Assert.That(conditionNode.GetProperty("skipped").GetBoolean()).IsTrue();
            await Assert.That(conditionNode.GetProperty("skipReason").GetString())
                .Contains(nameof(NeverRunCondition));
            await Assert.That(dependentNode.GetProperty("skipped").GetBoolean()).IsTrue();
            await Assert.That(dependentNode.GetProperty("skipReason").GetString())
                .Contains(nameof(ConditionSkippedModule));
            await Assert.That(_executions).IsEqualTo(0);
        }
    }

    [Test]
    public async Task Configured_Skip_Conditions_And_Their_Cascade_Are_Annotated()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule<ConfiguredSkippedModule>();
        builder.AddModule<DependentOnConfiguredSkippedModule>();
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        using var document = JsonDocument.Parse(
            await exporter.RenderAsync(DependencyGraphFormat.Json));
        var nodes = document.RootElement.GetProperty("nodes").EnumerateArray().ToArray();
        var configuredNode = nodes.Single(node =>
            node.GetProperty("name").GetString() == nameof(ConfiguredSkippedModule));
        var dependentNode = nodes.Single(node =>
            node.GetProperty("name").GetString() == nameof(DependentOnConfiguredSkippedModule));

        using (Assert.Multiple())
        {
            await Assert.That(configuredNode.GetProperty("skipped").GetBoolean()).IsTrue();
            await Assert.That(configuredNode.GetProperty("skipReason").GetString())
                .IsEqualTo("configured skip");
            await Assert.That(dependentNode.GetProperty("skipped").GetBoolean()).IsTrue();
            await Assert.That(dependentNode.GetProperty("skipReason").GetString())
                .Contains(nameof(ConfiguredSkippedModule));
        }
    }

    [Test]
    public async Task Result_Dependent_Configured_Skips_Are_Annotated_As_Unresolved()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule<DependencyModule>();
        builder.AddModule<ResultDependentConfiguredSkipModule>();
        builder.AddModule<DependentOnUnresolvedSkipModule>();
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        using var document = JsonDocument.Parse(
            await exporter.RenderAsync(DependencyGraphFormat.Json));
        var nodes = document.RootElement.GetProperty("nodes").EnumerateArray().ToArray();
        var configuredNode = nodes.Single(node =>
            node.GetProperty("name").GetString() == nameof(ResultDependentConfiguredSkipModule));
        var dependentNode = nodes.Single(node =>
            node.GetProperty("name").GetString() == nameof(DependentOnUnresolvedSkipModule));

        using (Assert.Multiple())
        {
            await Assert.That(configuredNode.GetProperty("skipped").ValueKind)
                .IsEqualTo(JsonValueKind.Null);
            await Assert.That(dependentNode.GetProperty("skipped").ValueKind)
                .IsEqualTo(JsonValueKind.Null);
        }
    }

    [Test]
    public async Task Async_Configured_Skip_Is_Unresolved_Without_Starting_Work()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule<AsyncConfiguredSkipModule>();
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        using var document = JsonDocument.Parse(
            await exporter.RenderAsync(DependencyGraphFormat.Json));
        var node = document.RootElement.GetProperty("nodes").EnumerateArray().Single();

        using (Assert.Multiple())
        {
            await Assert.That(node.GetProperty("skipped").ValueKind)
                .IsEqualTo(JsonValueKind.Null);
            await Assert.That(_asyncSkipConditionEvaluations).IsEqualTo(0);
        }
    }

    [Test]
    public async Task Synchronous_Skip_Short_Circuits_Before_Async_Condition()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule<SynchronouslySkippedBeforeAsyncModule>();
        builder.AddModule<DependentOnSynchronouslySkippedBeforeAsyncModule>();
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        using var document = JsonDocument.Parse(
            await exporter.RenderAsync(DependencyGraphFormat.Json));
        var nodes = document.RootElement.GetProperty("nodes").EnumerateArray().ToArray();
        var configuredNode = nodes.Single(node =>
            node.GetProperty("name").GetString() == nameof(SynchronouslySkippedBeforeAsyncModule));
        var dependentNode = nodes.Single(node =>
            node.GetProperty("name").GetString() == nameof(DependentOnSynchronouslySkippedBeforeAsyncModule));

        using (Assert.Multiple())
        {
            await Assert.That(configuredNode.GetProperty("skipped").GetBoolean()).IsTrue();
            await Assert.That(configuredNode.GetProperty("skipReason").GetString())
                .IsEqualTo("synchronous short circuit");
            await Assert.That(dependentNode.GetProperty("skipped").GetBoolean()).IsTrue();
            await Assert.That(_asyncSkipConditionEvaluations).IsEqualTo(0);
        }
    }

    [Test]
    public async Task Synchronous_Skip_Short_Circuits_After_Async_Condition()
    {
        _asyncSkipConditionEvaluations = 0;
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule<SynchronouslySkippedAfterAsyncModule>();
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        using var document = JsonDocument.Parse(
            await exporter.RenderAsync(DependencyGraphFormat.Json));
        var node = document.RootElement.GetProperty("nodes").EnumerateArray().Single();

        using (Assert.Multiple())
        {
            await Assert.That(node.GetProperty("skipped").GetBoolean()).IsTrue();
            await Assert.That(node.GetProperty("skipReason").GetString())
                .IsEqualTo("synchronous short circuit");
            await Assert.That(_asyncSkipConditionEvaluations).IsEqualTo(0);
        }
    }

    [Test]
    public async Task Render_Rejects_Invalid_Registration_Dependency()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule<InvalidDynamicDependencyModule>();
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        await Assert.ThrowsAsync<ModuleNotRegisteredException>(
            () => exporter.RenderAsync(DependencyGraphFormat.Json));
    }

    [Test]
    public async Task Render_Validates_Dependency_Before_Configured_Skip()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule<InvalidSkippedDynamicDependencyModule>();
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        await Assert.ThrowsAsync<ModuleNotRegisteredException>(
            () => exporter.RenderAsync(DependencyGraphFormat.Json));
    }

    [Test]
    public async Task Render_Includes_Registered_Dynamic_Dependency()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule<DependencyModule>();
        builder.AddModule<DynamicDependencyModule>();
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        using var document = JsonDocument.Parse(
            await exporter.RenderAsync(DependencyGraphFormat.Json));
        var nodes = document.RootElement.GetProperty("nodes").EnumerateArray().ToArray();
        var dependencyId = nodes.Single(node =>
            node.GetProperty("name").GetString() == nameof(DependencyModule)).GetProperty("id").GetString();
        var dependentId = nodes.Single(node =>
            node.GetProperty("name").GetString() == nameof(DynamicDependencyModule)).GetProperty("id").GetString();
        var edge = document.RootElement.GetProperty("edges").EnumerateArray().Single();

        using (Assert.Multiple())
        {
            await Assert.That(edge.GetProperty("from").GetString()).IsEqualTo(dependencyId);
            await Assert.That(edge.GetProperty("to").GetString()).IsEqualTo(dependentId);
        }
    }

    [Test]
    public async Task Render_Cascades_Skipped_Dynamic_Dependency()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.ConfigurePipelineOptions(options => options with
        {
            SkippedModules = [nameof(DependencyModule)],
        });
        builder.AddModule<DependencyModule>();
        builder.AddModule<DynamicDependencyModule>();
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        using var document = JsonDocument.Parse(
            await exporter.RenderAsync(DependencyGraphFormat.Json));
        var dependentNode = document.RootElement.GetProperty("nodes").EnumerateArray()
            .Single(node => node.GetProperty("name").GetString() == nameof(DynamicDependencyModule));

        using (Assert.Multiple())
        {
            await Assert.That(dependentNode.GetProperty("skipped").GetBoolean()).IsTrue();
            await Assert.That(dependentNode.GetProperty("skipReason").GetString())
                .Contains(nameof(DependencyModule));
        }
    }

    [Test]
    public async Task Render_Does_Not_Cache_Conditions_Before_Startup_Hooks()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule<StartupConditionModule>();
        builder.AddPipelineGlobalHooks<EnableStartupConditionHook>();
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        using var document = JsonDocument.Parse(
            await exporter.RenderAsync(DependencyGraphFormat.Json));
        var summary = await pipeline.RunAsync();

        using (Assert.Multiple())
        {
            await Assert.That(document.RootElement.GetProperty("nodes")[0]
                    .GetProperty("skipped").GetBoolean())
                .IsTrue();
            await Assert.That(summary.Results.Single().ModuleStatus).IsEqualTo(Status.Successful);
            await Assert.That(_executions).IsEqualTo(1);
        }
    }

    [Test]
    public async Task Render_Does_Not_Cache_Registration_Before_Startup_Hooks()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule<DependencyModule>();
        builder.AddModule<StartupDynamicDependencyModule>();
        builder.AddPipelineGlobalHooks<EnableStartupDependencyHook>();
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();
        var dependencyRegistry = pipeline.Services.GetRequiredService<IModuleDependencyRegistry>();

        _ = await exporter.RenderAsync(DependencyGraphFormat.Json);
        var dependenciesBeforeRun = dependencyRegistry
            .GetDynamicDependencies(typeof(StartupDynamicDependencyModule))
            .ToArray();
        var summary = await pipeline.RunAsync();
        var dynamicDependencies = dependencyRegistry
            .GetDynamicDependencies(typeof(StartupDynamicDependencyModule))
            .ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(dependenciesBeforeRun).IsEmpty();
            await Assert.That(summary.Results).Count().IsEqualTo(2);
            await Assert.That(dynamicDependencies).Contains(typeof(DependencyModule));
        }
    }

    [Test]
    public async Task Render_Does_Not_Freeze_Direct_Module_Configuration()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule(new StartupConfiguredModule());
        builder.AddPipelineGlobalHooks<EnableStartupConfigurationHook>();
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        using var document = JsonDocument.Parse(
            await exporter.RenderAsync(DependencyGraphFormat.Json));
        var summary = await pipeline.RunAsync();

        using (Assert.Multiple())
        {
            await Assert.That(document.RootElement.GetProperty("nodes")[0]
                    .GetProperty("skipped").GetBoolean())
                .IsTrue();
            await Assert.That(summary.Results.Single().ModuleStatus).IsEqualTo(Status.Successful);
            await Assert.That(_executions).IsEqualTo(1);
        }
    }

    [Test]
    public async Task Render_Does_Not_Freeze_Factory_Module_Configuration()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule(_ => new StartupConfiguredModule());
        builder.AddPipelineGlobalHooks<EnableStartupConfigurationHook>();
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        using var document = JsonDocument.Parse(
            await exporter.RenderAsync(DependencyGraphFormat.Json));
        var summary = await pipeline.RunAsync();

        using (Assert.Multiple())
        {
            await Assert.That(document.RootElement.GetProperty("nodes")[0]
                    .GetProperty("skipped").GetBoolean())
                .IsTrue();
            await Assert.That(summary.Results.Single().ModuleStatus).IsEqualTo(Status.Successful);
            await Assert.That(_executions).IsEqualTo(1);
        }
    }

    [Test]
    public async Task Planning_Conditions_Use_The_Supplied_Metadata_Registry()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule<DependencyModule>();
        await using var pipeline = await builder.BuildAsync();
        var conditionHandler = pipeline.Services.GetRequiredService<IModuleConditionHandler>();
        var runtimeRegistry = pipeline.Services.GetRequiredService<IModuleMetadataRegistry>();
        var planningRegistry = new ModuleMetadataRegistry(new ModuleAttributeEventService());
        var module = new PlanningCategoryModule();

        _ = await conditionHandler.ShouldIgnoreForPlanning(module, planningRegistry);

        using (Assert.Multiple())
        {
            await Assert.That(planningRegistry.GetCategory(typeof(PlanningCategoryModule)))
                .IsEqualTo("planning");
            await Assert.That(runtimeRegistry.GetCategory(typeof(PlanningCategoryModule))).IsNull();
        }
    }

    [Test]
    public async Task Render_Does_Not_Share_Mutable_Module_State_With_Runtime()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule<MutableConfigurationStateModule>();
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        _ = await exporter.RenderAsync(DependencyGraphFormat.Json);
        var summary = await pipeline.RunAsync();

        await Assert.That(summary.Results.Single().ModuleStatus).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task Render_Preserves_User_Factory_Initialization()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule<DependencyModule>();
        builder.AddModule(_ => new FactoryInitializedModule { IncludeDependency = true });
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        using var document = JsonDocument.Parse(
            await exporter.RenderAsync(DependencyGraphFormat.Json));

        await Assert.That(document.RootElement.GetProperty("edges").GetArrayLength()).IsEqualTo(1);
    }

    [Test]
    public async Task Render_Matches_User_Factory_To_Derived_Runtime_Type()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule<DependencyModule>();
        builder.AddModule<FactoryInitializedBaseModule>(
            _ => new FactoryInitializedDerivedModule { IncludeDependency = true });
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        using var document = JsonDocument.Parse(
            await exporter.RenderAsync(DependencyGraphFormat.Json));

        await Assert.That(document.RootElement.GetProperty("edges").GetArrayLength()).IsEqualTo(1);
    }

    [Test]
    public async Task Render_Rejects_Factory_Replay_With_Different_Initialization()
    {
        var factoryCalls = 0;
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule<DependencyModule>();
        builder.AddModule(_ => new FactoryInitializedModule
        {
            IncludeDependency = Interlocked.Increment(ref factoryCalls) == 1,
        });
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        var exception = await Assert.ThrowsAsync<PipelineException>(
            () => exporter.RenderAsync(DependencyGraphFormat.Json));

        await Assert.That(exception!.Message).Contains("Override CreatePlanningCopy");
    }

    [Test]
    public async Task Render_Rejects_Factory_Replay_With_Shared_Mutable_State()
    {
        var sharedState = new List<string>();
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule(_ => new SharedMutableFactoryStateModule(sharedState));
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();
        var configurationCountBeforeExport = sharedState.Count;

        var exception = await Assert.ThrowsAsync<PipelineException>(
            () => exporter.RenderAsync(DependencyGraphFormat.Json));

        await Assert.That(exception!.Message).Contains("Override CreatePlanningCopy");
        await Assert.That(sharedState).Count().IsEqualTo(configurationCountBeforeExport);
    }

    [Test]
    public async Task Render_Rejects_Factory_Replay_With_Struct_Wrapped_Shared_State()
    {
        var sharedState = new List<string>();
        var state = new StructWrappedState(sharedState);
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule(_ => new StructWrappedFactoryStateModule(state));
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();
        var configurationCountBeforeExport = sharedState.Count;

        var exception = await Assert.ThrowsAsync<PipelineException>(
            () => exporter.RenderAsync(DependencyGraphFormat.Json));

        await Assert.That(exception!.Message).Contains("Override CreatePlanningCopy");
        await Assert.That(sharedState).Count().IsEqualTo(configurationCountBeforeExport);
    }

    [Test]
    public async Task Render_Selects_Planning_Copy_Override_By_Signature()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule(_ => new OverloadedPlanningCopyModule());
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        var graph = await exporter.RenderAsync(DependencyGraphFormat.Json);

        await Assert.That(graph).Contains(nameof(OverloadedPlanningCopyModule));
    }

    [Test]
    public async Task Render_Accepts_Independent_Fieldless_State()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule(_ => new FieldlessStateFactoryModule());
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        var graph = await exporter.RenderAsync(DependencyGraphFormat.Json);

        await Assert.That(graph).Contains(nameof(FieldlessStateFactoryModule));
    }

    [Test]
    public async Task Render_Activates_Isolated_Direct_Interface_Module()
    {
        _directModuleActivations = 0;
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule(new DirectInterfaceModule());
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        _ = await exporter.RenderAsync(DependencyGraphFormat.Json);

        await Assert.That(_directModuleActivations).IsEqualTo(2);
    }

    [Test]
    public async Task Render_Activates_Isolated_Direct_Interface_Singleton_Factory_Module()
    {
        _directModuleActivations = 0;
        using var builder = Pipeline.CreateBuilder();
        builder.Services.AddSingleton<DirectInterfaceModule>();
        builder.AddModule(serviceProvider =>
            serviceProvider.GetRequiredService<DirectInterfaceModule>());
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        _ = await exporter.RenderAsync(DependencyGraphFormat.Json);

        await Assert.That(_directModuleActivations).IsEqualTo(2);
    }

    [Test]
    public async Task Render_Accepts_Service_Provider_Owned_Factory_State()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.Services.AddSingleton(new PlanningSingletonDependency
        {
            IncludeDependency = true,
        });
        builder.AddModule<DependencyModule>();
        builder.AddModule(serviceProvider => new ServiceBackedFactoryModule(
            serviceProvider.GetRequiredService<PlanningSingletonDependency>()));
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        using var document = JsonDocument.Parse(
            await exporter.RenderAsync(DependencyGraphFormat.Json));

        await Assert.That(document.RootElement.GetProperty("edges").GetArrayLength()).IsEqualTo(1);
    }

    [Test]
    public async Task Render_Rejects_Different_Inherited_Factory_State()
    {
        var factoryCalls = 0;
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule<DependencyModule>();
        builder.AddModule(_ => new InheritedSettingsFactoryModule(new DerivedFactorySettings
        {
            IncludeDependency = Interlocked.Increment(ref factoryCalls) == 1,
        }));
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        var exception = await Assert.ThrowsAsync<PipelineException>(
            () => exporter.RenderAsync(DependencyGraphFormat.Json));

        await Assert.That(exception!.Message).Contains("Override CreatePlanningCopy");
    }

    [Test]
    public async Task Render_Rejects_Different_Factory_Alias_Topology()
    {
        var factoryCalls = 0;
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule<DependencyModule>();
        builder.AddModule(_ =>
        {
            var first = new AliasState { IncludeDependency = true };
            var second = Interlocked.Increment(ref factoryCalls) == 1
                ? first
                : new AliasState { IncludeDependency = true };
            return new AliasTopologyFactoryModule(first, second);
        });
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        var exception = await Assert.ThrowsAsync<PipelineException>(
            () => exporter.RenderAsync(DependencyGraphFormat.Json));

        await Assert.That(exception!.Message).Contains("Override CreatePlanningCopy");
    }

    [Test]
    public async Task Render_Rejects_Different_Factory_Array_Shape()
    {
        var factoryCalls = 0;
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule<DependencyModule>();
        builder.AddModule(_ => new ArrayShapeFactoryModule(
            Interlocked.Increment(ref factoryCalls) == 1
                ? Array.CreateInstance(typeof(int), [1, 2], [0, 0])
                : Array.CreateInstance(typeof(int), [2, 1], [1, 0])));
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        var exception = await Assert.ThrowsAsync<PipelineException>(
            () => exporter.RenderAsync(DependencyGraphFormat.Json));

        await Assert.That(exception!.Message).Contains("Override CreatePlanningCopy");
    }

    [Test]
    public async Task Render_Accepts_Framework_Comparer_In_Independent_Factory_State()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule<DependencyModule>();
        builder.AddModule(_ => new ComparerBackedFactoryModule(
            new HashSet<string>(StringComparer.OrdinalIgnoreCase)));
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        using var document = JsonDocument.Parse(
            await exporter.RenderAsync(DependencyGraphFormat.Json));

        await Assert.That(document.RootElement.GetProperty("edges").GetArrayLength()).IsEqualTo(1);
    }

    [Test]
    public async Task Render_Clones_Precreated_Module_With_NonResolvable_Constructor_State()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule<DependencyModule>();
        builder.AddModule(new PrecreatedConfiguredModule(new PrecreatedModuleSettings
        {
            IncludeDependency = true,
        }));
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        using var document = JsonDocument.Parse(
            await exporter.RenderAsync(DependencyGraphFormat.Json));

        await Assert.That(document.RootElement.GetProperty("edges").GetArrayLength()).IsEqualTo(1);
    }

    [Test]
    public async Task Render_Rejects_Shared_Precreated_State_Without_Disposing_It()
    {
        var state = new PrecreatedDisposableState();
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule(new PrecreatedDisposableModule(state));
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        var exception = await Assert.ThrowsAsync<PipelineException>(
            () => exporter.RenderAsync(DependencyGraphFormat.Json));
        await Assert.That(state.IsDisposed).IsFalse();

        var summary = await pipeline.RunAsync();
        var moduleResult = await summary.Modules
            .OfType<PrecreatedDisposableModule>()
            .Single();

        using (Assert.Multiple())
        {
            await Assert.That(exception!.Message).Contains("Override CreatePlanningCopy");
            await Assert.That(moduleResult.ModuleStatus).IsEqualTo(Status.Successful);
        }
    }

    [Test]
    public async Task Render_Rejects_Shared_Comparer_With_Captured_State()
    {
        var comparisons = 0;
        var comparer = Comparer<string>.Create((left, right) =>
        {
            comparisons++;
            return StringComparer.Ordinal.Compare(left, right);
        });
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule<DependencyModule>();
        builder.AddModule(_ => new CapturingComparerFactoryModule(comparer));
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();
        var comparisonsBeforeRender = comparisons;

        var exception = await Assert.ThrowsAsync<PipelineException>(
            () => exporter.RenderAsync(DependencyGraphFormat.Json));

        using (Assert.Multiple())
        {
            await Assert.That(exception!.Message).Contains("Override CreatePlanningCopy");
            await Assert.That(comparisons).IsEqualTo(comparisonsBeforeRender);
        }
    }

    [Test]
    public async Task Render_Uses_Planning_Copy_After_Factory_Replay_Mismatch()
    {
        var factoryCalls = 0;
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule<DependencyModule>();
        builder.AddModule(_ => new FactoryInitializedPlanningCopyModule
        {
            IncludeDependency = Interlocked.Increment(ref factoryCalls) == 1,
        });
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        using var document = JsonDocument.Parse(
            await exporter.RenderAsync(DependencyGraphFormat.Json));

        await Assert.That(document.RootElement.GetProperty("edges").GetArrayLength()).IsEqualTo(1);
    }

    [Test]
    public async Task Render_Rejects_Factory_Replay_With_Different_Runtime_Type()
    {
        var factoryCalls = 0;
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule<FactoryInitializedBaseModule>(_ =>
            Interlocked.Increment(ref factoryCalls) == 1
                ? new FactoryInitializedDerivedModule()
                : new FactoryInitializedBaseModule());
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        var exception = await Assert.ThrowsAsync<PipelineException>(
            () => exporter.RenderAsync(DependencyGraphFormat.Json));

        await Assert.That(exception!.Message).Contains("Override CreatePlanningCopy");
    }

    [Test]
    public async Task Render_Rejects_Factory_Replay_With_Different_Skip_Decision()
    {
        var factoryCalls = 0;
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule(_ => new FactorySkipModule(
            Interlocked.Increment(ref factoryCalls) == 1));
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        var exception = await Assert.ThrowsAsync<PipelineException>(
            () => exporter.RenderAsync(DependencyGraphFormat.Json));

        await Assert.That(exception!.Message).Contains("Override CreatePlanningCopy");
    }

    [Test]
    public async Task Render_Disposes_Scoped_Planning_Module_After_Each_Render()
    {
        _planningDisposals = 0;
        using var builder = Pipeline.CreateBuilder();
        builder.Services.AddTransient<ContainerOwnedPlanningModule>();
        builder.AddModule(serviceProvider =>
            serviceProvider.GetRequiredService<ContainerOwnedPlanningModule>());

        await using (var pipeline = await builder.BuildAsync())
        {
            var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();
            _ = await exporter.RenderAsync(DependencyGraphFormat.Json);
            await Assert.That(_planningDisposals).IsEqualTo(1);

            _ = await exporter.RenderAsync(DependencyGraphFormat.Json);
            await Assert.That(_planningDisposals).IsEqualTo(2);
        }

        await Assert.That(_planningDisposals).IsGreaterThan(2);
    }

    [Test]
    public async Task Render_Disposes_Indirectly_Resolved_Scoped_Planning_Module()
    {
        _planningDisposals = 0;
        using var builder = Pipeline.CreateBuilder();
        builder.Services.AddTransient<ContainerOwnedPlanningModule>();
        builder.Services.AddTransient<ContainerOwnedPlanningModuleFactory>();
        builder.AddModule(serviceProvider => serviceProvider
            .GetRequiredService<ContainerOwnedPlanningModuleFactory>()
            .Create());

        await using (var pipeline = await builder.BuildAsync())
        {
            var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();
            _ = await exporter.RenderAsync(DependencyGraphFormat.Json);

            await Assert.That(_planningDisposals).IsEqualTo(1);
        }

        await Assert.That(_planningDisposals).IsGreaterThan(1);
    }

    [Test]
    public async Task Render_Does_Not_Dispose_Root_Provider_Owned_Planning_Module()
    {
        _planningDisposals = 0;
        using var builder = Pipeline.CreateBuilder();
        builder.Services.AddTransient<ContainerOwnedPlanningModule>();
        builder.Services.AddSingleton<ContainerOwnedPlanningModuleFactory>();
        builder.AddModule(serviceProvider => serviceProvider
            .GetRequiredService<ContainerOwnedPlanningModuleFactory>()
            .Create());

        await using (var pipeline = await builder.BuildAsync())
        {
            var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();
            _ = await exporter.RenderAsync(DependencyGraphFormat.Json);

            await Assert.That(_planningDisposals).IsEqualTo(0);
        }

        await Assert.That(_planningDisposals).IsGreaterThan(0);
    }

    [Test]
    public async Task Render_Accepts_Indirectly_Resolved_Container_State()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.Services.AddSingleton<ContainerOwnedPlanningState>();
        builder.Services.AddTransient<ContainerOwnedPlanningStateFactory>();
        builder.AddModule(serviceProvider => new ModuleWithContainerOwnedPlanningState(
            serviceProvider.GetRequiredService<ContainerOwnedPlanningStateFactory>().Create()));
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        using var document = JsonDocument.Parse(
            await exporter.RenderAsync(DependencyGraphFormat.Json));

        await Assert.That(document.RootElement.GetProperty("nodes").GetArrayLength())
            .IsEqualTo(1);
    }

    [Test]
    public async Task Render_Disposes_Scoped_Planning_Copy()
    {
        _planningDisposals = 0;
        using var builder = Pipeline.CreateBuilder();
        builder.Services.AddKeyedTransient<ContainerOwnedPlanningCopyModule>("planning");
        builder.AddModule(new ContainerOwnedPlanningCopyModule());

        await using (var pipeline = await builder.BuildAsync())
        {
            var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();
            _ = await exporter.RenderAsync(DependencyGraphFormat.Json);

            await Assert.That(_planningDisposals).IsEqualTo(1);
        }

        await Assert.That(_planningDisposals).IsEqualTo(1);
    }

    [Test]
    public async Task Render_Disposes_Manually_Constructed_Registered_Planning_Module()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.Services.AddTransient<ContainerOwnedPlanningModule>();
        builder.Services.AddSingleton<PlanningFactoryDependency>();
        builder.AddModule(serviceProvider =>
        {
            _ = serviceProvider.GetRequiredService<PlanningFactoryDependency>();
            return new ContainerOwnedPlanningModule();
        });
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();
        _planningDisposals = 0;

        _ = await exporter.RenderAsync(DependencyGraphFormat.Json);

        await Assert.That(_planningDisposals).IsEqualTo(1);
    }

    [Test]
    public async Task Render_Uses_Isolated_Copy_When_Factory_Returns_Runtime_Singleton()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.Services.AddSingleton<SingletonFactoryModule>();
        builder.AddModule(serviceProvider =>
            serviceProvider.GetRequiredService<SingletonFactoryModule>());
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        using var document = JsonDocument.Parse(
            await exporter.RenderAsync(DependencyGraphFormat.Json));

        await Assert.That(document.RootElement.GetProperty("nodes").GetArrayLength()).IsEqualTo(1);
    }

    [Test]
    public async Task Render_Disposes_Isolated_Planning_Modules()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule<DisposablePlanningModule>();
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();
        _planningDisposals = 0;

        _ = await exporter.RenderAsync(DependencyGraphFormat.Json);

        await Assert.That(_planningDisposals).IsEqualTo(1);
    }

    [Test]
    public async Task Canceled_Render_Does_Not_Activate_Or_Register_Planning_Modules()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule<DisposablePlanningModule>();
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();
        _planningActivations = 0;
        _planningDisposals = 0;
        _planningRegistrationEvents = 0;
        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(
            () => exporter.RenderAsync(
                DependencyGraphFormat.Json,
                cancellationTokenSource.Token));

        using (Assert.Multiple())
        {
            await Assert.That(_planningActivations).IsEqualTo(0);
            await Assert.That(_planningDisposals).IsEqualTo(0);
            await Assert.That(_planningRegistrationEvents).IsEqualTo(0);
        }
    }

    [Test]
    public async Task Render_Observes_Cancellation_After_Loading_Estimates()
    {
        using var cancellationTokenSource = new CancellationTokenSource();
        using var builder = Pipeline.CreateBuilder();
        builder.Services.AddSingleton<IModuleEstimatedTimeProvider>(
            new CancelingEstimatedTimeProvider(cancellationTokenSource));
        builder.AddModule<DependencyModule>();
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        await Assert.ThrowsAsync<OperationCanceledException>(
            () => exporter.RenderAsync(
                DependencyGraphFormat.Json,
                cancellationTokenSource.Token));
    }

    [Test]
    public async Task Completed_Summary_Render_Observes_An_Already_Canceled_Token()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule<DependencyModule>();
        await using var pipeline = await builder.BuildAsync();
        var summary = await pipeline.RunAsync();
        var renderer = pipeline.Services
            .GetRequiredService<IDependencyGraphExporter>();
        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(
            () => renderer.RenderSummaryAsync(
                DependencyGraphFormat.Json,
                summary,
                cancellationTokenSource.Token));
    }

    [Test]
    public async Task Render_Uses_Fresh_Stateful_Condition_Attributes()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule<SingleUseConditionModule>();
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        _ = await exporter.RenderAsync(DependencyGraphFormat.Json);
        var summary = await pipeline.RunAsync();

        await Assert.That(summary.Results.Single().ModuleStatus).IsEqualTo(Status.Successful);
    }

    [Test]
    public async Task Render_Can_Retry_After_Canceled_Module_Discovery()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule<DependencyModule>();
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();
        using var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(
            () => exporter.RenderAsync(
                DependencyGraphFormat.Json,
                cancellationTokenSource.Token));

        using var document = JsonDocument.Parse(
            await exporter.RenderAsync(DependencyGraphFormat.Json));
        await Assert.That(document.RootElement.GetProperty("nodes").GetArrayLength()).IsEqualTo(1);
    }

    [Test]
    public async Task Dot_Escapes_Line_Breaks_Inside_Label_Values()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule<LineBreakCategoryModule>();
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        var dot = await exporter.RenderAsync(DependencyGraphFormat.Dot);

        await Assert.That(dot).Contains(@"Category: build\nrelease");
    }

    [Test]
    public async Task Mermaid_Escapes_Markdown_Fence_Content()
    {
        using var builder = Pipeline.CreateBuilder();
        builder.AddModule<MarkdownFenceCategoryModule>();
        await using var pipeline = await builder.BuildAsync();
        var exporter = pipeline.Services.GetRequiredService<IDependencyGraphExporter>();

        var mermaid = await exporter.RenderAsync(DependencyGraphFormat.Mermaid);

        using (Assert.Multiple())
        {
            await Assert.That(mermaid).DoesNotContain("\n```");
            await Assert.That(mermaid).Contains("&#96;&#96;&#96;");
            await Assert.That(mermaid).Contains("build<br/>&#96;&#96;&#96;<br/>release");
        }
    }

    private static PipelineBuilder CreateBuilder()
    {
        var builder = Pipeline.CreateBuilder();
        builder.Services.AddSingleton<IModuleEstimatedTimeProvider, FixedEstimatedTimeProvider>();
        builder.AddModule<DependencyModule>();
        builder.AddModule<TargetModule>();
        builder.AddModule<SkippedModule>();
        builder.ConfigurePipelineOptions(options => options with
        {
            SkippedModules = [nameof(DependencyModule)],
        });
        return builder;
    }
}
