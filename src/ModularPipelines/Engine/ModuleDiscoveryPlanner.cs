using System.Diagnostics.CodeAnalysis;
using System.Reflection;
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

        var planningModules = new IModule[_modules.Count];
        var ownedPlanningModules = new List<IModule>();
        try
        {
            var originalModules = new Dictionary<IModule, IModule>(ReferenceEqualityComparer.Instance);
            for (var index = 0; index < planningModules.Length; index++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var planningModule = CreatePlanningModule(_modules[index], ownedPlanningModules);
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

            var dependencyRegistry = new ModuleDependencyRegistry();
            var attributeEventService = new ModuleAttributeEventService();
            var metadataRegistry = new ModuleMetadataRegistry(attributeEventService);
            var dependencyChainProvider = new DependencyChainProvider(
                metadataRegistry,
                dependencyRegistry);
            var registrationEventExecutor = new RegistrationEventExecutor(
                attributeEventService,
                attributeEventInvoker,
                dependencyRegistry,
                metadataRegistry,
                configuration,
                environment);
            cancellationToken.ThrowIfCancellationRequested();
            await registrationEventExecutor.InvokeRegistrationEventsAsync(
                    planningModules)
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
                    .ShouldIgnoreByCategory(module, metadataRegistry, cancellationToken)
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
                dependencyRegistry,
                metadataRegistry,
                originalModules,
                ownedPlanningModules);
        }
        catch
        {
            await DisposePlanningModulesAsync(ownedPlanningModules).ConfigureAwait(false);
            throw;
        }
    }

    private PlanningModule CreatePlanningModule(
        IModule module,
        ICollection<IModule> ownedPlanningModules)
    {
        var factory = _planningFactories.LastOrDefault(candidate =>
            candidate.CreatedRuntimeModule(module));
        if (factory is null)
        {
            return CreatePlanningModuleWithoutFactory(module);
        }

        var creation = factory.CreatePlanningModule(serviceProvider);
        if (ReferenceEquals(creation.Module, module))
        {
            return module is IPlanningModuleCopyProvider factoryCopyProvider
                ? CreateIsolatedPlanningModule(CreatePlanningCopy(factoryCopyProvider))
                : CreatePlanningModuleWithoutFactory(module);
        }

        if (module is IPlanningModuleCopyProvider replayCopyProvider
            && HasCustomPlanningCopy(module)
            && !IsEquivalentPlanningModule(
                module,
                creation.Module,
                creation.IsServiceProviderOwned))
        {
            if (creation.IsPlannerOwned)
            {
                ownedPlanningModules.Add(creation.Module);
            }

            return CreateIsolatedPlanningModule(CreatePlanningCopy(replayCopyProvider));
        }

        return CreateIsolatedPlanningModule(creation);
    }

    private PlanningModule CreatePlanningModuleWithoutFactory(IModule module)
    {
        if (module is IPlanningModuleCopyProvider copyProvider)
        {
            return CreateIsolatedPlanningModule(CreatePlanningCopy(copyProvider));
        }

        var trackingServiceProvider = new ResolvedObjectTrackingServiceProvider(serviceProvider);
        try
        {
            var planningModule = serviceProvider
                .GetRequiredService<IModuleActivator>()
                .CreateModule(module.GetType(), trackingServiceProvider);
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
        && HasEquivalentModuleState(
            runtimeModule,
            planningModule,
            isServiceProviderOwned);

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
            || (type.IsValueType && !ContainsReferenceFields(type)))
        {
            return first.Equals(second);
        }

        if (type.IsValueType)
        {
            return HasEquivalentFields(first, second, context);
        }

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
            return context.IsServiceProviderOwned(first)
                   || first is Array { Length: 0 }
                   || IsKnownImmutableFrameworkSingleton(first)
                   || (IsFrameworkComparer(first)
                       && HasEquivalentFields(first, first, context))
                   || (first is Delegate sharedDelegate
                       && HasEquivalentDelegates(sharedDelegate, sharedDelegate, context));
        }

        return HasEquivalentReferenceState(first, second, context);
    }

    [UnconditionalSuppressMessage(
        "ReflectionAnalysis",
        "IL2070",
        Justification = "Value-type fields are inspected only to detect embedded shared references during planning validation.")]
    private static bool ContainsReferenceFields(Type type)
    {
        if (!type.IsValueType)
        {
            return true;
        }

        if (type.IsPrimitive || type.IsEnum || type.IsPointer)
        {
            return false;
        }

        return type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Any(field => ContainsReferenceFields(field.FieldType));
    }

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
            : HasEquivalentFields(first, second, context);
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

    private PlanningModuleCreation CreatePlanningCopy(IPlanningModuleCopyProvider copyProvider)
    {
        var trackingServiceProvider = new ResolvedObjectTrackingServiceProvider(serviceProvider);
        var module = copyProvider.CreatePlanningCopy(trackingServiceProvider);
        return new PlanningModuleCreation(
            module,
            trackingServiceProvider.IsServiceProviderOwned,
            IsPlannerOwned: !trackingServiceProvider.IsServiceProviderOwned(module));
    }

    internal static async Task DisposePlanningModulesAsync(IEnumerable<IModule> planningModules)
    {
        foreach (var module in planningModules
                     .Distinct<IModule>(ReferenceEqualityComparer.Instance)
                     .Reverse())
        {
            await Disposer.DisposeObjectAsync(module).ConfigureAwait(false);
        }
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
    private readonly IModule? _registeredInstance;
    private IModule? _runtimeModule;

    public ModulePlanningFactory(
        Func<IServiceProvider, IModule> create,
        IModule? registeredInstance = null)
    {
        _create = create;
        _registeredInstance = registeredInstance;
        _runtimeModule = registeredInstance;
    }

    public IModule CreateRuntimeModule(IServiceProvider serviceProvider)
    {
        var module = _create(serviceProvider);
        Interlocked.CompareExchange(ref _runtimeModule, module, null);
        return module;
    }

    public bool CreatedRuntimeModule(IModule module) =>
        ReferenceEquals(Volatile.Read(ref _runtimeModule), module);

    public PlanningModuleCreation CreatePlanningModule(IServiceProvider serviceProvider)
    {
        if (_registeredInstance is IPlanningModuleCopyProvider copyProvider)
        {
            if (ModuleDiscoveryPlanner.HasCustomPlanningCopy(_registeredInstance))
            {
                var customCopyServiceProvider = new ResolvedObjectTrackingServiceProvider(serviceProvider);
                var customCopy = copyProvider.CreatePlanningCopy(customCopyServiceProvider);
                return new PlanningModuleCreation(
                    customCopy,
                    customCopyServiceProvider.IsServiceProviderOwned,
                    IsPlannerOwned: !customCopyServiceProvider.IsServiceProviderOwned(customCopy));
            }

            return new PlanningModuleCreation(
                copyProvider.CreatePlanningCopyFromRegisteredInstance(),
                static _ => false,
                IsPlannerOwned: false);
        }

        var trackingServiceProvider = new ResolvedObjectTrackingServiceProvider(serviceProvider);
        var module = _create(trackingServiceProvider);
        return new PlanningModuleCreation(
            module,
            trackingServiceProvider.IsServiceProviderOwned,
            IsPlannerOwned: !trackingServiceProvider.IsServiceProviderOwned(module));
    }
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
        return _resolvedObjects.Contains(value) || IsTrackedDisposable(value);
    }

    [UnconditionalSuppressMessage(
        "ReflectionAnalysis",
        "IL2075",
        Justification = "Microsoft DI's internal disposal list is inspected only to establish exact instance ownership.")]
    private bool IsTrackedDisposable(object value)
    {
        var disposablesField = innerServiceProvider.GetType().GetField(
            "_disposables",
            BindingFlags.Instance | BindingFlags.NonPublic);
        return disposablesField?.GetValue(innerServiceProvider) is IEnumerable<object> disposables
               && disposables.Any(disposable => ReferenceEquals(disposable, value));
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
    IReadOnlyList<IModule> OwnedPlanningModules) : IAsyncDisposable
{
    public async ValueTask DisposeAsync()
    {
        await ModuleDiscoveryPlanner
            .DisposePlanningModulesAsync(OwnedPlanningModules)
            .ConfigureAwait(false);
    }
}
