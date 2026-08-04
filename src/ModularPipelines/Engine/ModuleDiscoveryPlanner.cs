using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ModularPipelines.Configuration;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Exceptions;
using ModularPipelines.Helpers;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Engine;

internal sealed class ModuleDiscoveryPlanner(
    IModuleConditionHandler moduleConditionHandler,
    IAttributeEventInvoker attributeEventInvoker,
    IConfiguration configuration,
    IHostEnvironment environment,
    IEnumerable<IModule> modules,
    ISafeModuleEstimatedTimeProvider estimatedTimeProvider,
    IOptions<PipelineOptions> pipelineOptions,
    IServiceProvider serviceProvider,
    IEnumerable<ModulePlanningFactory> planningFactories)
{
    private const byte LoadInstanceFieldAddressOpCode = 0x7C;
    private const byte StoreInstanceFieldOpCode = 0x7D;

    private static readonly OpCode[] OneByteOpCodes = CreateOpCodeLookup(twoByte: false);
    private static readonly OpCode[] TwoByteOpCodes = CreateOpCodeLookup(twoByte: true);

    private readonly IReadOnlyList<IModule> _modules = modules
        .Distinct<IModule>(ReferenceEqualityComparer.Instance)
        .ToArray();
    private readonly IReadOnlyList<ModulePlanningFactory> _planningFactories =
        planningFactories.ToArray();

    public async Task<PlannedModuleDiscovery> DiscoverAsync(
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        if (_modules.Count == 0)
        {
            throw new PipelineException("No modules have been registered");
        }

        var planningScope = serviceProvider.CreateAsyncScope();
        var planningServiceProvider = planningScope.ServiceProvider;
        var planningModules = new IModule[_modules.Count];
        var ownedPlanningModules = new List<IModule>();
        try
        {
            var originalModules = new Dictionary<IModule, IModule>(ReferenceEqualityComparer.Instance);
            for (var index = 0; index < planningModules.Length; index++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var planningModule = CreatePlanningModule(
                    _modules[index],
                    ownedPlanningModules,
                    planningServiceProvider);
                planningModules[index] = planningModule.Module;
                if (planningModule.IsPlannerOwned
                    && !ReferenceEquals(planningModule.Module, _modules[index]))
                {
                    ownedPlanningModules.Add(planningModule.Module);
                }

                if (planningModule.RequiresIsolation)
                {
                    ValidatePlanningModule(
                        _modules[index],
                        planningModule.Module,
                        planningModule.IsServiceProviderOwned);
                }
                originalModules.Add(planningModules[index], _modules[index]);
            }

            var planningDependencyRegistry = new ModuleDependencyRegistry();
            var attributeEventService = new ModuleAttributeEventService();
            var planningMetadataRegistry = new ModuleMetadataRegistry(
                attributeEventService,
                planningSafeOnly: true);
            var dependencyChainProvider = new DependencyChainProvider(
                planningMetadataRegistry,
                planningDependencyRegistry,
                planningSafeOnly: true);
            var registrationEventExecutor = new RegistrationEventExecutor(
                attributeEventService,
                attributeEventInvoker,
                planningDependencyRegistry,
                planningMetadataRegistry,
                configuration,
                environment,
                planningSafeOnly: true);
            cancellationToken.ThrowIfCancellationRequested();
            await registrationEventExecutor.InvokeRegistrationEventsAsync(planningModules)
                .ConfigureAwait(false);

            var moduleSelection = ModuleSelection.Create(
                planningModules,
                dependencyChainProvider,
                pipelineOptions.Value);
            var ignoredModules = new List<IgnoredModule>();
            var runnableModules = new List<IModule>();
            foreach (var module in planningModules)
            {
                cancellationToken.ThrowIfCancellationRequested();
                if (moduleSelection.GetSkipDecision(module) is { } selectionSkipDecision)
                {
                    ignoredModules.Add(new IgnoredModule(module, selectionSkipDecision));
                    continue;
                }

                var (shouldIgnore, skipDecision) = await moduleConditionHandler
                    .ShouldIgnoreByCategory(module, planningMetadataRegistry, cancellationToken)
                    .ConfigureAwait(false);
                if (shouldIgnore)
                {
                    ignoredModules.Add(new IgnoredModule(
                        module,
                        skipDecision ?? SkipDecision.Skip("Module was ignored")));
                }
                else
                {
                    runnableModules.Add(module);
                }
            }

            var runnableModulesWithEstimatedDuration = await Task.WhenAll(
                    runnableModules.Select(async module => new RunnableModule(
                        module,
                        await estimatedTimeProvider
                            .GetModuleEstimatedTimeAsync(module.GetType())
                            .ConfigureAwait(false))))
                .ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();

            return new PlannedModuleDiscovery(
                new OrganizedModules(runnableModulesWithEstimatedDuration, ignoredModules),
                dependencyChainProvider,
                planningDependencyRegistry,
                planningMetadataRegistry,
                originalModules,
                ownedPlanningModules,
                planningScope);
        }
        catch (Exception discoveryException)
        {
            var cleanupExceptions = await CollectPlanningCleanupExceptionsAsync(
                    ownedPlanningModules,
                    planningScope)
                .ConfigureAwait(false);
            if (cleanupExceptions.Count > 0)
            {
                throw new AggregateException(
                    "Module discovery and planning cleanup both failed.",
                    new[] { discoveryException }.Concat(cleanupExceptions));
            }

            throw;
        }
    }

    private PlanningModule CreatePlanningModule(
        IModule module,
        ICollection<IModule> ownedPlanningModules,
        IServiceProvider planningServiceProvider)
    {
        var factory = _planningFactories.LastOrDefault(candidate =>
            candidate.CreatedRuntimeModule(module));
        if (factory is null)
        {
            return CreatePlanningModuleWithoutFactory(
                module,
                ownedPlanningModules,
                planningServiceProvider);
        }

        if (module is IPlanningModuleCopyProvider copyProvider)
        {
            if (HasCustomPlanningCopy(module))
            {
                return CreateIsolatedPlanningModule(CreatePlanningCopy(
                    module,
                    copyProvider,
                    ownedPlanningModules,
                    planningServiceProvider));
            }

            RejectRuntimeBoundPlanningCondition(module, copyProvider);
            return CreateIsolatedPlanningModule(new PlanningModuleCreation(
                copyProvider.CreatePlanningCopyFromRegisteredInstance(
                    preserveConfiguration: true),
                factory.IsRuntimeServiceProviderOwned,
                IsPlannerOwned: false));
        }

        return CreatePlanningModuleWithoutFactory(
            module,
            ownedPlanningModules,
            planningServiceProvider);
    }

    private static PlanningModule CreatePlanningModuleWithoutFactory(
        IModule module,
        ICollection<IModule> ownedPlanningModules,
        IServiceProvider planningServiceProvider)
    {
        if (module is IPlanningModuleCopyProvider copyProvider)
        {
            if (!HasCustomPlanningCopy(module)
                && ModuleActivator.TryGetRuntimeServiceOwnership(
                    module,
                    out var runtimeServiceOwnership))
            {
                if (TryCreatePlanningCopyFromRegisteredInstance(
                        module,
                        copyProvider,
                        runtimeServiceOwnership,
                        out var registeredInstanceCopy))
                {
                    return CreateIsolatedPlanningModule(registeredInstanceCopy);
                }

                throw new PipelineException(
                    $"The registered module '{module.GetType().FullName}' contains runtime state that "
                    + "cannot be shared with dependency-graph planning. Override CreatePlanningCopy to "
                    + "return an isolated copy.");
            }

            return CreateIsolatedPlanningModule(CreatePlanningCopy(
                module,
                copyProvider,
                ownedPlanningModules,
                planningServiceProvider));
        }

        var trackingServiceProvider = new ResolvedObjectTrackingServiceProvider(planningServiceProvider);
        try
        {
            var planningModule = planningServiceProvider
                .GetRequiredService<IModuleActivator>()
                .CreatePlanningModule(module.GetType(), trackingServiceProvider);
            return CreateIsolatedPlanningModule(new PlanningModuleCreation(
                planningModule,
                trackingServiceProvider.IsServiceProviderOwned,
                IsPlannerOwned: !trackingServiceProvider.IsServiceProviderOwned(planningModule)));
        }
        catch (Exception exception)
        {
            throw new PipelineException(
                $"The module '{module.GetType().FullName}' does not provide a planning copy and could not be " +
                "activated as an isolated dependency-graph planning instance.",
                exception);
        }
    }

    private static PlanningModule CreateIsolatedPlanningModule(PlanningModuleCreation creation) =>
        new(
            creation.Module,
            creation.IsPlannerOwned,
            RequiresIsolation: true,
            creation.IsServiceProviderOwned);

    private static void ValidatePlanningModule(
        IModule runtimeModule,
        IModule planningModule,
        Func<object, bool> isServiceProviderOwned)
    {
        if (ReferenceEquals(planningModule, runtimeModule))
        {
            throw new PipelineException(
                $"The module factory for '{runtimeModule.GetType().FullName}' returned the runtime instance " +
                "during dependency-graph planning. The factory must return a fresh module instance.");
        }

        if (runtimeModule is not IPlanningModuleCopyProvider
            && HasServiceProviderOwnedState(planningModule, isServiceProviderOwned))
        {
            throw new PipelineException(
                $"The direct module '{runtimeModule.GetType().FullName}' shares service-provider-owned state " +
                "with its dependency-graph planning instance. Derive from Module<T> and override " +
                "CreatePlanningCopy to return an isolated copy.");
        }

        if (!IsEquivalentPlanningModule(
                runtimeModule,
                planningModule,
                isServiceProviderOwned))
        {
            throw new PipelineException(
                $"The planning copy for '{runtimeModule.GetType().FullName}' did not preserve the runtime " +
                "module type and dependency-graph configuration. Override CreatePlanningCopy to " +
                "return an initialized, isolated copy.");
        }
    }

    private static bool IsEquivalentPlanningModule(
        IModule runtimeModule,
        IModule planningModule,
        Func<object, bool> isServiceProviderOwned) =>
        planningModule.GetType() == runtimeModule.GetType()
        && (runtimeModule is not IPlanningModuleCopyProvider
            || HasEquivalentPlanningConfiguration(
                runtimeModule.Configuration,
                planningModule.Configuration,
                isServiceProviderOwned))
        && HasEquivalentModuleState(
            runtimeModule,
            planningModule,
            isServiceProviderOwned);

    private static bool HasEquivalentPlanningConfiguration(
        ModuleConfiguration runtimeConfiguration,
        ModuleConfiguration planningConfiguration,
        Func<object, bool> isServiceProviderOwned)
    {
        if (ReferenceEquals(runtimeConfiguration, planningConfiguration))
        {
            return true;
        }

        var context = new StateComparisonContext(isServiceProviderOwned);
        return runtimeConfiguration.Dependencies.SequenceEqual(planningConfiguration.Dependencies)
               && runtimeConfiguration.Tags.SetEquals(planningConfiguration.Tags)
               && runtimeConfiguration.Category == planningConfiguration.Category
               && HasEquivalentState(
                   runtimeConfiguration.SynchronousPlanningSkipCondition,
                   planningConfiguration.SynchronousPlanningSkipCondition,
                   context);
    }

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2075",
        Justification = "The already-loaded runtime module type is inspected only to detect a planning-copy override.")]
    internal static bool HasCustomPlanningCopy(IModule module)
    {
        var method = module.GetType().GetMethod(
            "CreatePlanningCopy",
            BindingFlags.Instance | BindingFlags.NonPublic,
            binder: null,
            [typeof(IServiceProvider)],
            modifiers: null);
        var declaringType = method?.DeclaringType;
        return declaringType is not null
               && (!declaringType.IsGenericType
                   || declaringType.GetGenericTypeDefinition() != typeof(Module<>));
    }

    [UnconditionalSuppressMessage(
        "ReflectionAnalysis",
        "IL2075",
        Justification = "Factory-created module state is inspected only to verify planning replay equivalence.")]
    private static bool HasEquivalentModuleState(
        IModule runtimeModule,
        IModule planningModule,
        Func<object, bool> isServiceProviderOwned)
    {
        var context = new StateComparisonContext(isServiceProviderOwned);
        for (var type = runtimeModule.GetType();
             type is not null && (!type.IsGenericType
                                  || type.GetGenericTypeDefinition() != typeof(Module<>));
             type = type.BaseType)
        {
            foreach (var field in type.GetFields(
                         BindingFlags.Instance
                         | BindingFlags.Public
                         | BindingFlags.NonPublic
                         | BindingFlags.DeclaredOnly))
            {
                if (!HasEquivalentState(
                        field.GetValue(runtimeModule),
                        field.GetValue(planningModule),
                        context))
                {
                    return false;
                }
            }
        }

        return true;
    }

    private static bool HasEquivalentState(
        object? first,
        object? second,
        StateComparisonContext context)
    {
        if (first is null || second is null)
        {
            return first is null && second is null;
        }

        return first.GetType() == second.GetType()
               && HasEquivalentNonNullState(first, second, context);
    }

    private static bool HasEquivalentNonNullState(
        object first,
        object second,
        StateComparisonContext context)
    {
        var type = first.GetType();
        if (first is string or Type
            || type.IsPrimitive
            || type.IsEnum
            || type.IsPointer)
        {
            return first.Equals(second);
        }

        if (type.IsValueType)
        {
            return HasEquivalentFields(first, second, context);
        }

        return HasEquivalentReferenceMapping(first, second, context);
    }

    private static bool HasEquivalentReferenceMapping(
        object first,
        object second,
        StateComparisonContext context)
    {
        var mapping = context.Map(first, second);
        if (mapping == ReferenceMapping.Mismatch)
        {
            return false;
        }

        if (mapping == ReferenceMapping.Existing)
        {
            return true;
        }

        if (ReferenceEquals(first, second))
        {
            return HasEquivalentSharedReference(first, context);
        }

        return HasEquivalentReferenceState(first, second, context);
    }

    private static bool HasEquivalentSharedReference(
        object value,
        StateComparisonContext context) =>
        context.IsServiceProviderOwned(value)
        || value is Array { Length: 0 }
        || (value is not Array && !HasInstanceFields(value))
        || IsKnownImmutableFrameworkSingleton(value)
        || (IsFrameworkComparer(value)
            && HasEquivalentFields(value, value, context))
        || (value is Delegate sharedDelegate
            && HasEquivalentDelegates(sharedDelegate, sharedDelegate, context));

    private static bool HasEquivalentReferenceState(
        object first,
        object second,
        StateComparisonContext context)
    {
        if (first is Delegate firstDelegate && second is Delegate secondDelegate)
        {
            return HasEquivalentDelegates(firstDelegate, secondDelegate, context);
        }

        return first is Array firstArray && second is Array secondArray
            ? HasEquivalentArrays(firstArray, secondArray, context)
            : HasInstanceFields(first)
              && HasEquivalentFields(first, second, context);
    }

    [UnconditionalSuppressMessage(
        "ReflectionAnalysis",
        "IL2075",
        Justification = "Factory-created module state is inspected only to verify planning replay equivalence.")]
    private static bool HasInstanceFields(object value)
    {
        for (var current = value.GetType(); current is not null; current = current.BaseType)
        {
            if (current.GetFields(
                    BindingFlags.Instance
                    | BindingFlags.Public
                    | BindingFlags.NonPublic
                    | BindingFlags.DeclaredOnly).Length > 0)
            {
                return true;
            }
        }

        return false;
    }

    private static bool HasEquivalentDelegates(
        Delegate first,
        Delegate second,
        StateComparisonContext context)
    {
        var firstInvocationList = first.GetInvocationList();
        var secondInvocationList = second.GetInvocationList();
        return firstInvocationList.Length == secondInvocationList.Length
               && firstInvocationList.Zip(secondInvocationList)
                   .All(pair => pair.First.Method == pair.Second.Method
                                && HasEquivalentState(
                                    pair.First.Target,
                                    pair.Second.Target,
                                    context));
    }

    private static bool HasEquivalentArrays(
        Array first,
        Array second,
        StateComparisonContext context)
    {
        return first.Rank == second.Rank
               && Enumerable.Range(0, first.Rank)
                   .All(dimension =>
                       first.GetLength(dimension) == second.GetLength(dimension)
                       && first.GetLowerBound(dimension) == second.GetLowerBound(dimension))
               && first.Cast<object?>().Zip(second.Cast<object?>())
                   .All(pair => HasEquivalentState(pair.First, pair.Second, context));
    }

    private static bool IsKnownImmutableFrameworkSingleton(object value)
    {
        var type = value.GetType();
        if (type.Assembly != typeof(object).Assembly)
        {
            return false;
        }

        if (value is StringComparer)
        {
            return ReferenceEquals(value, StringComparer.Ordinal)
                   || ReferenceEquals(value, StringComparer.OrdinalIgnoreCase)
                   || ReferenceEquals(value, StringComparer.InvariantCulture)
                   || ReferenceEquals(value, StringComparer.InvariantCultureIgnoreCase);
        }

        return false;
    }

    [UnconditionalSuppressMessage(
        "ReflectionAnalysis",
        "IL2075",
        Justification = "Framework comparer interfaces are inspected only to validate shared factory state.")]
    private static bool IsFrameworkComparer(object value)
    {
        var type = value.GetType();
        return type.Assembly == typeof(object).Assembly
               && (value is System.Collections.IComparer
                   || value is System.Collections.IEqualityComparer
                   || type.GetInterfaces().Any(static interfaceType =>
                   {
                       if (!interfaceType.IsGenericType)
                       {
                           return false;
                       }

                       var definition = interfaceType.GetGenericTypeDefinition();
                       return definition == typeof(IComparer<>)
                              || definition == typeof(IEqualityComparer<>);
                   }));
    }

    [UnconditionalSuppressMessage(
        "ReflectionAnalysis",
        "IL2075",
        Justification = "Factory-created module state is inspected only to verify planning replay equivalence.")]
    private static bool HasEquivalentFields(
        object first,
        object second,
        StateComparisonContext context)
    {
        for (var type = first.GetType(); type is not null; type = type.BaseType)
        {
            foreach (var field in type.GetFields(
                         BindingFlags.Instance
                         | BindingFlags.Public
                         | BindingFlags.NonPublic
                         | BindingFlags.DeclaredOnly))
            {
                if (!HasEquivalentState(
                        field.GetValue(first),
                        field.GetValue(second),
                        context))
                {
                    return false;
                }
            }
        }

        return true;
    }

    private static PlanningModuleCreation CreatePlanningCopy(
        IModule runtimeModule,
        IPlanningModuleCopyProvider copyProvider,
        ICollection<IModule> ownedPlanningModules,
        IServiceProvider planningServiceProvider)
    {
        var trackingServiceProvider = new ResolvedObjectTrackingServiceProvider(planningServiceProvider);
        var module = copyProvider.CreatePlanningCopy(trackingServiceProvider);
        var isPlannerOwned = !trackingServiceProvider.IsServiceProviderOwned(module);
        try
        {
            if (module is IPlanningModuleCopyProvider planningCopyProvider)
            {
                if (!HasCustomPlanningCopy(runtimeModule)
                    && (HasServiceProviderOwnedState(
                            module,
                            trackingServiceProvider.IsServiceProviderOwned)
                        || (copyProvider.IsConfigurationInitialized
                            && ConfigurationTouchesStaticState(runtimeModule))))
                {
                    var runtimeConfiguration = runtimeModule.Configuration;
                    RejectRuntimeBoundPlanningCondition(runtimeModule, copyProvider);
                    planningCopyProvider.InitializeConfiguration(runtimeConfiguration);
                }
                else if (copyProvider.IsConfigurationInitialized)
                {
                    planningCopyProvider.InitializeConfiguration();
                }
            }
        }
        catch
        {
            if (isPlannerOwned)
            {
                ownedPlanningModules.Add(module);
            }

            throw;
        }

        return new PlanningModuleCreation(
            module,
            trackingServiceProvider.IsServiceProviderOwned,
            IsPlannerOwned: isPlannerOwned);
    }

    private static bool TryCreatePlanningCopyFromRegisteredInstance(
        IModule runtimeModule,
        IPlanningModuleCopyProvider copyProvider,
        ResolvedObjectTrackingServiceProvider runtimeServiceOwnership,
        [NotNullWhen(true)] out PlanningModuleCreation? creation)
    {
        var module = copyProvider.CreatePlanningCopyFromRegisteredInstance(
            preserveConfiguration: false);
        if (!HasEquivalentModuleState(
                runtimeModule,
                module,
                runtimeServiceOwnership.IsServiceProviderOwned))
        {
            creation = null;
            return false;
        }

        if (module is IPlanningModuleCopyProvider planningCopyProvider
            && copyProvider.IsConfigurationInitialized)
        {
            if (HasServiceProviderOwnedState(
                    module,
                    runtimeServiceOwnership.IsServiceProviderOwned)
                || ConfigurationTouchesStaticState(runtimeModule))
            {
                RejectRuntimeBoundPlanningCondition(runtimeModule, copyProvider);
                planningCopyProvider.InitializeConfiguration(runtimeModule.Configuration);
            }
            else
            {
                planningCopyProvider.InitializeConfiguration();
            }
        }

        creation = new PlanningModuleCreation(
            module,
            runtimeServiceOwnership.IsServiceProviderOwned,
            IsPlannerOwned: false);
        return true;
    }

    [UnconditionalSuppressMessage(
        "ReflectionAnalysis",
        "IL2075",
        Justification = "Planning module fields are inspected only to detect resolved service references.")]
    private static bool HasServiceProviderOwnedState(
        IModule module,
        Func<object, bool> isServiceProviderOwned)
    {
        var visited = new HashSet<object>(ReferenceEqualityComparer.Instance) { module };
        for (var type = module.GetType();
             type is not null && (!type.IsGenericType
                                  || type.GetGenericTypeDefinition() != typeof(Module<>));
             type = type.BaseType)
        {
            if (type.GetFields(
                    BindingFlags.Instance
                    | BindingFlags.Public
                    | BindingFlags.NonPublic
                    | BindingFlags.DeclaredOnly)
                .Select(field => field.GetValue(module))
                .Any(value => ContainsServiceProviderOwnedState(
                    value,
                    isServiceProviderOwned,
                    visited)))
            {
                return true;
            }
        }

        return false;
    }

    private static bool ContainsServiceProviderOwnedState(
        object? value,
        Func<object, bool> isServiceProviderOwned,
        ISet<object> visited)
    {
        if (value is null)
        {
            return false;
        }

        if (isServiceProviderOwned(value))
        {
            return true;
        }

        var type = value.GetType();
        if (ShouldSkipPlanningValue(value, type, visited))
        {
            return false;
        }

        if (value is Delegate @delegate)
        {
            return @delegate.GetInvocationList().Any(invocation =>
                ContainsServiceProviderOwnedState(
                    invocation.Target,
                    isServiceProviderOwned,
                    visited));
        }

        if (value is Array array)
        {
            return array.Cast<object?>().Any(item =>
                ContainsServiceProviderOwnedState(item, isServiceProviderOwned, visited));
        }

        return GetInstanceFields(type).Any(field =>
            ContainsServiceProviderOwnedState(
                field.GetValue(value),
                isServiceProviderOwned,
                visited));
    }

    private static void RejectRuntimeBoundPlanningCondition(
        IModule module,
        IPlanningModuleCopyProvider copyProvider)
    {
        if (!copyProvider.IsConfigurationInitialized
            || module.Configuration.SynchronousPlanningSkipCondition is not { } planningCondition)
        {
            return;
        }

        var referencesRuntimeModule = ReferencesObject(
            planningCondition,
            module,
            new HashSet<object>(ReferenceEqualityComparer.Instance));
        var mutatesCapturedState = MutatesDelegateTargetState(
            planningCondition,
            new HashSet<object>(ReferenceEqualityComparer.Instance));
        if (!referencesRuntimeModule && !mutatesCapturedState)
        {
            return;
        }

        throw new PipelineException(
            $"The initialized configuration for '{module.GetType().FullName}' contains a planning condition "
            + "bound to mutable runtime state. Override CreatePlanningCopy to return an isolated copy.");
    }

    [UnconditionalSuppressMessage(
        "ReflectionAnalysis",
        "IL2075",
        Justification = "Planning delegates are inspected only to reject mutable captured state.")]
    private static bool MutatesDelegateTargetState(object? value, ISet<object> visited)
    {
        if (value is null)
        {
            return false;
        }

        if (value is Delegate @delegate)
        {
            return DelegateTargetMutatesState(@delegate, visited);
        }

        var type = value.GetType();
        if (ShouldSkipPlanningValue(value, type, visited))
        {
            return false;
        }

        if (value is Array array)
        {
            return MutatesPlanningArray(array, visited);
        }

        return MutatesPlanningObject(value, type, visited);
    }

    private static bool MutatesPlanningArray(Array array, ISet<object> visited) =>
        !typeof(Delegate).IsAssignableFrom(array.GetType().GetElementType()!)
        || array.Cast<object?>().Any(item => MutatesDelegateTargetState(item, visited));

    private static bool MutatesPlanningObject(
        object value,
        Type type,
        ISet<object> visited) =>
        !type.IsDefined(typeof(CompilerGeneratedAttribute), inherit: false)
        || GetInstanceFields(type)
            .Any(field => MutatesDelegateTargetState(field.GetValue(value), visited));

    private static bool ShouldSkipPlanningValue(
        object value,
        Type type,
        ISet<object> visited) =>
        IsTerminalPlanningValue(value, type)
        || (!type.IsValueType && !visited.Add(value));

    private static bool DelegateTargetMutatesState(Delegate @delegate, ISet<object> visited) =>
        @delegate.GetInvocationList().Any(invocation =>
            (!IsKnownPlanningSafeDelegateWrapper(invocation.Method)
             && MethodTouchesStaticState(
                 invocation.Method,
                 new HashSet<MethodBase>()))
            || (invocation.Target is { } target
                && (MutatesDelegateTargetField(invocation.Method, target.GetType())
                    || MutatesDelegateTargetState(target, visited))));

    private static bool IsKnownPlanningSafeDelegateWrapper(MethodInfo method) =>
        method.Module.Assembly == typeof(ModuleConfiguration).Assembly
        && method.DeclaringType?.IsDefined(
            typeof(CompilerGeneratedAttribute),
            inherit: false) == true;

    private static bool IsTerminalPlanningValue(object value, Type type) =>
        value is string or MemberInfo or SkipDecision
        || type.IsPrimitive
        || type.IsEnum
        || type.IsPointer;

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2026",
        Justification = "Missing or trimmed method bodies are conservatively treated as mutable.")]
    [UnconditionalSuppressMessage(
        "ReflectionAnalysis",
        "IL2070",
        Justification = "Delegate target fields are inspected only to reject mutable captured state.")]
    [UnconditionalSuppressMessage(
        "ReflectionAnalysis",
        "IL2075",
        Justification = "Delegate target base fields are inspected only to reject mutable captured state.")]
    private static bool MutatesDelegateTargetField(MethodInfo method, Type targetType)
    {
        var il = method.GetMethodBody()?.GetILAsByteArray();
        if (il is null)
        {
            return true;
        }

        foreach (var field in GetInstanceFields(targetType))
        {
            var token = BitConverter.GetBytes(field.MetadataToken);
            for (var index = 0; index <= il.Length - token.Length - 1; index++)
            {
                if ((il[index] == StoreInstanceFieldOpCode
                     || il[index] == LoadInstanceFieldAddressOpCode)
                    && il.AsSpan(index + 1, token.Length).SequenceEqual(token))
                {
                    return true;
                }
            }
        }

        return false;
    }

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2075",
        Justification = "The already-loaded module Configure method is inspected only to avoid replaying global state.")]
    private static bool ConfigurationTouchesStaticState(IModule module)
    {
        var method = module.GetType().GetMethod(
            "Configure",
            BindingFlags.Instance | BindingFlags.NonPublic);
        return method is not null
               && MethodTouchesStaticState(method, new HashSet<MethodBase>());
    }

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2026",
        Justification = "Missing or trimmed Configure bodies are conservatively treated as stateful.")]
    private static bool MethodTouchesStaticState(
        MethodBase method,
        ISet<MethodBase> visited)
    {
        if (!visited.Add(method))
        {
            return false;
        }

        var il = method.GetMethodBody()?.GetILAsByteArray();
        if (il is null)
        {
            return true;
        }

        var offset = 0;
        while (offset < il.Length)
        {
            var opCode = ReadOpCode(il, ref offset);
            if (opCode.Size == 0)
            {
                return true;
            }

            var operandOffset = offset;
            var operandSize = GetOperandSize(opCode, il, operandOffset);
            if (operandOffset + operandSize > il.Length)
            {
                return true;
            }

            if (operandSize == sizeof(int)
                && InstructionTouchesStaticState(
                    method,
                    opCode,
                    il,
                    operandOffset,
                    visited))
            {
                return true;
            }

            offset += operandSize;
        }

        return false;
    }

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2026",
        Justification = "Missing or trimmed Configure metadata is conservatively treated as stateful.")]
    private static bool InstructionTouchesStaticState(
        MethodBase method,
        OpCode opCode,
        byte[] il,
        int operandOffset,
        ISet<MethodBase> visited)
    {
        var token = BitConverter.ToInt32(il, operandOffset);
        var methodGenericArguments = method is MethodInfo methodInfo
            ? methodInfo.GetGenericArguments()
            : null;
        try
        {
            if (opCode.OperandType == OperandType.InlineField)
            {
                var field = method.Module.ResolveField(
                    token,
                    method.DeclaringType?.GetGenericArguments(),
                    methodGenericArguments);
                return field?.IsStatic == true
                       && !IsKnownPlanningSafeConfigurationField(field);
            }

            if (opCode.OperandType != OperandType.InlineMethod
                || method.Module.ResolveMethod(
                    token,
                    method.DeclaringType?.GetGenericArguments(),
                    methodGenericArguments) is not { } calledMethod)
            {
                return false;
            }

            if (calledMethod is ConstructorInfo
                && calledMethod.DeclaringType?.IsDefined(
                    typeof(CompilerGeneratedAttribute),
                    inherit: false) == true)
            {
                return false;
            }

            if (calledMethod.Module.Assembly == method.Module.Assembly)
            {
                return MethodTouchesStaticState(calledMethod, visited);
            }

            return calledMethod is ConstructorInfo
                   || !IsKnownPlanningSafeConfigurationMethod(calledMethod);
        }
        catch (ArgumentException)
        {
            return true;
        }
    }

    private static bool IsKnownPlanningSafeConfigurationMethod(MethodBase method) =>
        method.DeclaringType == typeof(ModuleConfiguration)
        || method.DeclaringType == typeof(ModuleConfigurationBuilder)
        || method.DeclaringType == typeof(AdvancedModuleConfigurationBuilder)
        || method.DeclaringType == typeof(SkipDecision);

    private static bool IsKnownPlanningSafeConfigurationField(FieldInfo field) =>
        field.DeclaringType == typeof(SkipDecision) && field.IsInitOnly;

    private static OpCode ReadOpCode(byte[] il, ref int offset)
    {
        var firstByte = il[offset++];
        if (firstByte != 0xFE)
        {
            return OneByteOpCodes[firstByte];
        }

        return offset < il.Length ? TwoByteOpCodes[il[offset++]] : default;
    }

    private static int GetOperandSize(OpCode opCode, byte[] il, int operandOffset) =>
        opCode.OperandType switch
        {
            OperandType.InlineNone => 0,
            OperandType.ShortInlineBrTarget or OperandType.ShortInlineI or OperandType.ShortInlineVar => 1,
            OperandType.InlineVar => 2,
            OperandType.InlineBrTarget
                or OperandType.InlineField
                or OperandType.InlineI
                or OperandType.InlineMethod
                or OperandType.InlineSig
                or OperandType.InlineString
                or OperandType.InlineTok
                or OperandType.ShortInlineR => 4,
            OperandType.InlineI8 or OperandType.InlineR => 8,
            OperandType.InlineSwitch when operandOffset + sizeof(int) <= il.Length =>
                sizeof(int) + (BitConverter.ToInt32(il, operandOffset) * sizeof(int)),
            _ => il.Length - operandOffset,
        };

    private static OpCode[] CreateOpCodeLookup(bool twoByte)
    {
        var lookup = new OpCode[byte.MaxValue + 1];
        foreach (var field in typeof(OpCodes).GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            if (field.GetValue(null) is not OpCode opCode)
            {
                continue;
            }

            var value = unchecked((ushort) opCode.Value);
            if (twoByte == (value > byte.MaxValue))
            {
                lookup[value & byte.MaxValue] = opCode;
            }
        }

        return lookup;
    }

    [UnconditionalSuppressMessage(
        "ReflectionAnalysis",
        "IL2075",
        Justification = "Planning delegates are inspected only to reject references to the runtime module.")]
    private static bool ReferencesObject(
        object? value,
        object target,
        ISet<object> visited,
        bool inspectFields = false)
    {
        if (ReferenceEquals(value, target))
        {
            return true;
        }

        if (value is null)
        {
            return false;
        }

        var type = value.GetType();
        if (ShouldSkipPlanningValue(value, type, visited))
        {
            return false;
        }

        if (value is Delegate @delegate)
        {
            return @delegate.GetInvocationList().Any(invocation =>
                ReferencesObject(invocation.Target, target, visited, inspectFields: true));
        }

        if (value is Array array)
        {
            return array.Cast<object?>().Any(item =>
                ReferencesObject(item, target, visited, inspectFields: true));
        }

        return ReferencesObjectFields(value, target, visited, inspectFields, type);
    }

    private static bool ReferencesObjectFields(
        object value,
        object target,
        ISet<object> visited,
        bool inspectFields,
        Type type)
    {
        var inspectCollectionFields = value is IEnumerable;
        if (!inspectFields
            && !inspectCollectionFields
            && !type.IsDefined(typeof(CompilerGeneratedAttribute), inherit: false))
        {
            return false;
        }

        return GetInstanceFields(type).Any(field => ReferencesObject(
            field.GetValue(value),
            target,
            visited,
            inspectCollectionFields));
    }

    [UnconditionalSuppressMessage(
        "ReflectionAnalysis",
        "IL2070",
        Justification = "Planning state is inspected only to validate isolated planning copies.")]
    [UnconditionalSuppressMessage(
        "ReflectionAnalysis",
        "IL2075",
        Justification = "Planning state base fields are inspected only to validate isolated planning copies.")]
    private static IEnumerable<FieldInfo> GetInstanceFields(Type type)
    {
        for (var current = type; current is not null; current = current.BaseType)
        {
            foreach (var field in current.GetFields(
                         BindingFlags.Instance
                         | BindingFlags.Public
                         | BindingFlags.NonPublic
                         | BindingFlags.DeclaredOnly))
            {
                yield return field;
            }
        }
    }

    internal static async Task DisposePlanningModulesAsync(IEnumerable<IModule> planningModules)
    {
        var exceptions = new List<Exception>();
        foreach (var module in planningModules
                     .Distinct<IModule>(ReferenceEqualityComparer.Instance)
                     .Reverse())
        {
            try
            {
                await Disposer.DisposeObjectAsync(module).ConfigureAwait(false);
            }
            catch (Exception exception) when (exception is not (OutOfMemoryException or StackOverflowException))
            {
                exceptions.Add(exception);
            }
        }

        if (exceptions.Count > 0)
        {
            throw new AggregateException("One or more planning modules failed to dispose", exceptions);
        }
    }

    internal static async Task<IReadOnlyList<Exception>> CollectPlanningCleanupExceptionsAsync(
        IEnumerable<IModule> planningModules,
        AsyncServiceScope planningScope)
    {
        var exceptions = new List<Exception>();
        try
        {
            await DisposePlanningModulesAsync(planningModules).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            exceptions.Add(exception);
        }

        try
        {
            await planningScope.DisposeAsync().ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            exceptions.Add(exception);
        }

        return exceptions;
    }

    private sealed class StateComparisonContext(Func<object, bool> isServiceProviderOwned)
    {
        private readonly Dictionary<object, object> _firstToSecond =
            new(ReferenceEqualityComparer.Instance);
        private readonly Dictionary<object, object> _secondToFirst =
            new(ReferenceEqualityComparer.Instance);

        public bool IsServiceProviderOwned(object value) => isServiceProviderOwned(value);

        public ReferenceMapping Map(object first, object second)
        {
            if (_firstToSecond.TryGetValue(first, out var mappedSecond))
            {
                return ReferenceEquals(mappedSecond, second)
                    ? ReferenceMapping.Existing
                    : ReferenceMapping.Mismatch;
            }

            if (_secondToFirst.ContainsKey(second))
            {
                return ReferenceMapping.Mismatch;
            }

            _firstToSecond.Add(first, second);
            _secondToFirst.Add(second, first);
            return ReferenceMapping.Added;
        }
    }

    private enum ReferenceMapping
    {
        Added,
        Existing,
        Mismatch,
    }
}

internal sealed class ModulePlanningFactory
{
    private readonly Func<IServiceProvider, IModule> _create;
    private IModule? _runtimeModule;
    private ResolvedObjectTrackingServiceProvider? _runtimeServiceOwnership;

    public ModulePlanningFactory(
        Func<IServiceProvider, IModule> create,
        IModule? registeredInstance = null)
    {
        _create = create;
        _runtimeModule = registeredInstance;
    }

    public IModule CreateRuntimeModule(IServiceProvider serviceProvider)
    {
        var runtimeServiceOwnership = new ResolvedObjectTrackingServiceProvider(serviceProvider);
        var module = _create(runtimeServiceOwnership);
        if (Interlocked.CompareExchange(ref _runtimeModule, module, null) is null)
        {
            Volatile.Write(ref _runtimeServiceOwnership, runtimeServiceOwnership);
        }

        return module;
    }

    public bool CreatedRuntimeModule(IModule module) =>
        ReferenceEquals(Volatile.Read(ref _runtimeModule), module);

    public bool IsRuntimeServiceProviderOwned(object value) =>
        Volatile.Read(ref _runtimeServiceOwnership)?.IsServiceProviderOwned(value) == true;
}

internal sealed class ResolvedObjectTrackingServiceProvider(IServiceProvider innerServiceProvider)
    : IServiceProvider, IKeyedServiceProvider
{
    private readonly HashSet<object> _resolvedObjects = new(ReferenceEqualityComparer.Instance);

    public object? GetService(Type serviceType) => Track(innerServiceProvider.GetService(serviceType));

    public object? GetKeyedService(Type serviceType, object? serviceKey) =>
        Track(((IKeyedServiceProvider) innerServiceProvider).GetKeyedService(serviceType, serviceKey));

    public object GetRequiredKeyedService(Type serviceType, object? serviceKey) =>
        Track(((IKeyedServiceProvider) innerServiceProvider)
            .GetRequiredKeyedService(serviceType, serviceKey))!;

    public bool IsServiceProviderOwned(object value)
    {
        return _resolvedObjects.Contains(value)
               || IsTrackedDisposable(value)
               || IsCachedService(value);
    }

    private bool IsTrackedDisposable(object value)
    {
        if (ContainsTrackedDisposable(innerServiceProvider, value))
        {
            return true;
        }

        var rootScope = GetRootScope();
        return rootScope is not null
               && !ReferenceEquals(rootScope, innerServiceProvider)
               && ContainsTrackedDisposable(rootScope, value);
    }

    [UnconditionalSuppressMessage(
        "ReflectionAnalysis",
        "IL2075",
        Justification = "Microsoft DI's internal disposal list is inspected only to establish exact instance ownership.")]
    private static bool ContainsTrackedDisposable(object scope, object value)
    {
        var disposablesField = scope.GetType().GetField(
            "_disposables",
            BindingFlags.Instance | BindingFlags.NonPublic);
        return disposablesField?.GetValue(scope) is IEnumerable<object> disposables
               && disposables.Any(disposable => ReferenceEquals(disposable, value));
    }

    [UnconditionalSuppressMessage(
        "ReflectionAnalysis",
        "IL2075",
        Justification = "Microsoft DI's internal scope caches are inspected only to establish exact instance ownership.")]
    private bool IsCachedService(object value)
    {
        if (ContainsResolvedService(innerServiceProvider, value))
        {
            return true;
        }

        var rootProvider = GetRootProvider();
        var rootScope = GetRootScope();
        return (rootScope is not null
                && !ReferenceEquals(rootScope, innerServiceProvider)
                && ContainsResolvedService(rootScope, value))
               || ContainsSingletonCallSiteValue(rootProvider, value);
    }

    private object GetRootProvider() =>
        GetPropertyValue(innerServiceProvider, "RootProvider") ?? innerServiceProvider;

    private object? GetRootScope() =>
        GetPropertyValue(innerServiceProvider, "Root")
        ?? GetPropertyValue(GetRootProvider(), "Root");

    private static bool ContainsResolvedService(object scope, object value)
    {
        var resolvedServices = GetPropertyValue(scope, "ResolvedServices");
        return GetPropertyValue(resolvedServices, "Values") is System.Collections.IEnumerable values
               && values.Cast<object?>().Any(resolvedService => ReferenceEquals(resolvedService, value));
    }

    private static bool ContainsSingletonCallSiteValue(object rootProvider, object value)
    {
        var callSiteFactory = GetPropertyValue(rootProvider, "CallSiteFactory");
        var callSiteCache = GetFieldValue(callSiteFactory, "_callSiteCache");
        return GetPropertyValue(callSiteCache, "Values") is System.Collections.IEnumerable callSites
               && callSites.Cast<object>().Any(callSite =>
                   ReferenceEquals(GetPropertyValue(callSite, "Value"), value)
                   || ReferenceEquals(GetPropertyValue(callSite, "DefaultValue"), value));
    }

    [UnconditionalSuppressMessage(
        "ReflectionAnalysis",
        "IL2075",
        Justification = "Microsoft DI's internal scope caches are inspected only to establish exact instance ownership.")]
    private static object? GetPropertyValue(object? target, string propertyName)
    {
        return target?.GetType()
            .GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            ?.GetValue(target);
    }

    [UnconditionalSuppressMessage(
        "ReflectionAnalysis",
        "IL2075",
        Justification = "Microsoft DI's internal call-site cache is inspected only to establish exact instance ownership.")]
    private static object? GetFieldValue(object? target, string fieldName)
    {
        return target?.GetType()
            .GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            ?.GetValue(target);
    }

    private object? Track(object? value)
    {
        if (value is not null)
        {
            _resolvedObjects.Add(value);
        }

        return value;
    }
}

internal sealed record PlanningModule(
    IModule Module,
    bool IsPlannerOwned,
    bool RequiresIsolation,
    Func<object, bool> IsServiceProviderOwned);

internal sealed record PlanningModuleCreation(
    IModule Module,
    Func<object, bool> IsServiceProviderOwned,
    bool IsPlannerOwned);

internal sealed record PlannedModuleDiscovery(
    OrganizedModules OrganizedModules,
    IDependencyChainProvider DependencyChainProvider,
    IModuleDependencyRegistry DependencyRegistry,
    IModuleMetadataRegistry MetadataRegistry,
    IReadOnlyDictionary<IModule, IModule> OriginalModules,
    IReadOnlyList<IModule> OwnedPlanningModules,
    AsyncServiceScope PlanningScope) : IAsyncDisposable
{
    public async ValueTask DisposeAsync()
    {
        var exceptions = await ModuleDiscoveryPlanner
            .CollectPlanningCleanupExceptionsAsync(OwnedPlanningModules, PlanningScope)
            .ConfigureAwait(false);
        if (exceptions.Count > 0)
        {
            throw new AggregateException("Dependency graph planning cleanup failed.", exceptions);
        }
    }
}
