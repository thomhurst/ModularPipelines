using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Distributed;
using ModularPipelines.Engine.Attributes;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Engine;

internal class ModuleConditionHandler : IModuleConditionHandler
{
    private readonly IOptions<PipelineOptions> _pipelineOptions;
    private readonly IPipelineContextProvider _pipelineContextProvider;
    private readonly IModuleMetadataRegistry _metadataRegistry;
    private readonly IExecutionLocationContext _executionLocationContext;
    private readonly LocalCapabilityRegistry? _localCapabilities;
    private readonly ConditionalWeakTable<IModule, ConditionEvaluation> _conditionEvaluations = new();
    private readonly ConcurrentDictionary<Type, Lazy<ConditionAttributes>> _conditionAttributes = new();

    public ModuleConditionHandler(
        IOptions<PipelineOptions> pipelineOptions,
        IPipelineContextProvider pipelineContextProvider,
        IModuleMetadataRegistry metadataRegistry,
        IExecutionLocationContext executionLocationContext,
        LocalCapabilityRegistry? localCapabilities = null)
    {
        _pipelineOptions = pipelineOptions;
        _pipelineContextProvider = pipelineContextProvider;
        _metadataRegistry = metadataRegistry;
        _executionLocationContext = executionLocationContext;
        _localCapabilities = localCapabilities;
    }

    public async Task<(bool ShouldIgnore, SkipDecision? SkipDecision)> ShouldIgnore(IModule module, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var evaluation = _conditionEvaluations.GetValue(module, static _ => new ConditionEvaluation());
        await evaluation.Gate.WaitAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            if (evaluation.HasResult)
            {
                return evaluation.Result;
            }

            var result = await EvaluateShouldIgnore(module, cancellationToken).ConfigureAwait(false);
            evaluation.Result = result;
            evaluation.HasResult = true;
            return result;
        }
        finally
        {
            evaluation.Gate.Release();
        }
    }

    public Task<(bool ShouldIgnore, SkipDecision? SkipDecision)> ShouldIgnoreByCategory(
        IModule module,
        CancellationToken cancellationToken = default)
        => ShouldIgnoreByCategory(module, _metadataRegistry, cancellationToken);

    public Task<(bool ShouldIgnore, SkipDecision? SkipDecision)> ShouldIgnoreByCategory(
        IModule module,
        IModuleMetadataRegistry metadataRegistry,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var result = EvaluateCategoryConditions(module, metadataRegistry);
        if (!result.ShouldIgnore
            && _executionLocationContext.ShouldDeferCapabilityConditions
            && CapabilityConditions.HasImpossibleCombination(module.GetType()))
        {
            result = (true, SkipDecision.Skip("Module requires mutually exclusive operating systems"));
        }

        return Task.FromResult(result);
    }

    public async Task PrepareExecutionRoutingAsync(
        IModule module,
        CancellationToken cancellationToken = default)
    {
        if (!_executionLocationContext.ShouldDeferCapabilityConditions)
        {
            return;
        }

        if (_executionLocationContext.IsRoutingPrepared(module))
        {
            return;
        }

        var attributes = GetConditionAttributes(module.GetType());
        var pipelineContext = _pipelineContextProvider.GetModuleContext();
        if (!await CanPrepareSkipConditionRoutingAsync(
                    attributes.Skip,
                    pipelineContext,
                    cancellationToken)
                .ConfigureAwait(false)
            || !await CanPrepareRequiredConditionRoutingAsync(
                    attributes.All,
                    pipelineContext,
                    cancellationToken)
                .ConfigureAwait(false))
        {
            _executionLocationContext.MarkRoutingPrepared(module);
            return;
        }

        _executionLocationContext.SetPreparedConditionValue(
            module,
            await PrepareConditionRoutingAsync(module, attributes, pipelineContext, cancellationToken)
                .ConfigureAwait(false));
        _executionLocationContext.MarkRoutingPrepared(module);
    }

    /// <summary>
    /// Evaluates the module's capability-bearing condition groups with the master's values for
    /// planning-safe conditions. Returns the capability requirement of the worker that must run the
    /// module, or false when no worker can satisfy its conditions. Other mandatory conditions already
    /// hold (see <see cref="CanPrepareRequiredConditionRoutingAsync"/>), and workers evaluate the rest.
    /// </summary>
    /// <remarks>
    /// Each planning-safe condition is evaluated at most once, lazily, with run-condition short-circuiting.
    /// A group is marked satisfied only when it holds without consulting any worker-only condition.
    /// </remarks>
    private async Task<FormulaValue> PrepareConditionRoutingAsync(
        IModule module,
        ConditionAttributes attributes,
        IPipelineContext pipelineContext,
        CancellationToken cancellationToken)
    {
        var values = new Dictionary<object, bool>(ReferenceEqualityComparer.Instance);
        var consultedWorkerOnlyCondition = false;
        async Task<bool?> EvaluatePlanningAtomAsync(ConditionAtom atom)
        {
            if (!atom.IsPlanning)
            {
                // A worker-only condition may hold on the worker, so it constrains nothing here. Treating it
                // as true also short-circuits an OR exactly where a worker would stop.
                consultedWorkerOnlyCondition = true;
                return true;
            }

            if (!values.TryGetValue(atom.Key, out var value))
            {
                cancellationToken.ThrowIfCancellationRequested();
                value = await atom.EvaluateConditionAsync(pipelineContext, cancellationToken).ConfigureAwait(false);
                values[atom.Key] = value;
            }

            return value;
        }

        var result = FormulaValue.True;
        foreach (var (conditionGroupType, formula) in ConditionFormula
                     .ForConditionGroups([.. attributes.All, .. attributes.Any])
                     .Where(static group => group.Formula.Capabilities.Any()))
        {
            consultedWorkerOnlyCondition = false;
            var value = await formula.EvaluateAsync(EvaluatePlanningAtomAsync).ConfigureAwait(false);
            if (value.Kind == FormulaValueKind.True && !consultedWorkerOnlyCondition)
            {
                // Local conditions alone satisfy the group, so the worker need not re-evaluate it.
                _executionLocationContext.MarkConditionGroupSatisfied(module, conditionGroupType);
            }

            result = FormulaValue.And(result, value);
            if (result.Kind == FormulaValueKind.False)
            {
                break;
            }
        }

        return result;
    }

    private static async Task<bool> CanPrepareSkipConditionRoutingAsync(
        IEnumerable<RunConditionAttribute> attributes,
        IPipelineContext pipelineContext,
        CancellationToken cancellationToken)
    {
        foreach (var attribute in attributes)
        {
            if (!IsPlanningConditionAttribute(attribute))
            {
                return false;
            }

            cancellationToken.ThrowIfCancellationRequested();
            if (await attribute.EvaluateAsync(pipelineContext, cancellationToken).ConfigureAwait(false))
            {
                return false;
            }
        }

        return true;
    }

    private static async Task<bool> CanPrepareRequiredConditionRoutingAsync(
        IEnumerable<RunConditionAttribute> attributes,
        IPipelineContext pipelineContext,
        CancellationToken cancellationToken)
    {
        foreach (var attribute in attributes.Where(static attribute =>
                     CapabilityConditions.GetRoute(attribute) is null))
        {
            if (!IsPlanningConditionAttribute(attribute))
            {
                return false;
            }

            cancellationToken.ThrowIfCancellationRequested();
            if (!await attribute.EvaluateAsync(pipelineContext, cancellationToken).ConfigureAwait(false))
            {
                return false;
            }
        }

        return true;
    }

    public Task<(bool ShouldIgnore, SkipDecision? SkipDecision)> ShouldIgnoreForPlanning(
        IModule module,
        CancellationToken cancellationToken = default)
        => ShouldIgnoreForPlanning(module, _metadataRegistry, cancellationToken);

    public Task<(bool ShouldIgnore, SkipDecision? SkipDecision)> ShouldIgnoreForPlanning(
        IModule module,
        IModuleMetadataRegistry metadataRegistry,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return EvaluateShouldIgnore(
            module,
            cancellationToken,
            useFreshAttributes: true,
            metadataRegistry);
    }

    public async Task<PlanningConditionResult> ShouldIgnoreForGraphPlanning(
        IModule module,
        IModuleMetadataRegistry metadataRegistry,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var categoryResult = EvaluateCategoryConditions(module, metadataRegistry);
        if (categoryResult.ShouldIgnore)
        {
            return new PlanningConditionResult(
                true,
                categoryResult.SkipDecision,
                IsResolved: true);
        }

        if (await EvaluateCapabilityRequirement(module, cancellationToken).ConfigureAwait(false) is { } capabilitySkip)
        {
            return new PlanningConditionResult(true, capabilitySkip, IsResolved: true);
        }

        return await EvaluatePlanningConditions(
                CreatePlanningConditionAttributes(module.GetType()),
                _pipelineContextProvider.GetModuleContext(),
                _executionLocationContext.ShouldDeferCapabilityConditions,
                cancellationToken)
            .ConfigureAwait(false);
    }

    private async Task<(bool ShouldIgnore, SkipDecision? SkipDecision)> EvaluateShouldIgnore(
        IModule module,
        CancellationToken cancellationToken,
        bool useFreshAttributes = false,
        IModuleMetadataRegistry? metadataRegistry = null)
    {
        var categoryResult = EvaluateCategoryConditions(module, metadataRegistry ?? _metadataRegistry);
        if (categoryResult.ShouldIgnore)
        {
            return categoryResult;
        }

        if (await EvaluateCapabilityRequirement(module, cancellationToken).ConfigureAwait(false) is { } capabilitySkip)
        {
            return (true, capabilitySkip);
        }

        var conditionResult = await IsRunnableCondition(
                module,
                cancellationToken,
                useFreshAttributes)
            .ConfigureAwait(false);
        return conditionResult.IsRunnable
            ? (false, null)
            : (true, conditionResult.SkipDecision);
    }

    private (bool ShouldIgnore, SkipDecision? SkipDecision) EvaluateCategoryConditions(IModule module)
        => EvaluateCategoryConditions(module, _metadataRegistry);

    /// <summary>
    /// Skips a module whose declared capability requirement this process cannot satisfy.
    /// The distributed master routes such modules to a capable worker instead.
    /// </summary>
    private async Task<SkipDecision?> EvaluateCapabilityRequirement(
        IModule module,
        CancellationToken cancellationToken)
    {
        if (_localCapabilities is null || _executionLocationContext.ShouldDeferCapabilityConditions)
        {
            return null;
        }

        var requirement = CapabilityConditions.GetDeclaredRequirement(module.GetType());
        if (requirement.IsEmpty)
        {
            return null;
        }

        var capabilities = await _localCapabilities.GetAsync(cancellationToken).ConfigureAwait(false);
        if (requirement.IsSatisfiedBy(capabilities))
        {
            return null;
        }

        return SkipDecision.Skip(
            $"Requires capabilities {requirement}, but this machine provides " +
            $"[{string.Join(", ", capabilities.Select(static capability => capability.Name).Order(StringComparer.OrdinalIgnoreCase))}]. " +
            "Declare capabilities with AddCapabilities(...) or register an ICapabilityProvider.");
    }

    private (bool ShouldIgnore, SkipDecision? SkipDecision) EvaluateCategoryConditions(
        IModule module,
        IModuleMetadataRegistry metadataRegistry)
    {
        var moduleType = module.GetType();
        metadataRegistry.FinalizeMetadata(moduleType, module);
        var category = metadataRegistry.GetCategory(moduleType);

        if (IsIgnoreCategory(category))
        {
            return (true, SkipDecision.Skip("A category of this module has been ignored"));
        }

        if (!IsRunnableCategory(category))
        {
            return (true, SkipDecision.Skip("The module was not in a runnable category"));
        }

        return (false, null);
    }

    private bool IsRunnableCategory(string? category)
    {
        var runOnlyCategories = _pipelineOptions.Value.RunOnlyCategories?.ToArray();

        if (runOnlyCategories is not { Length: > 0 })
        {
            return true;
        }

        return category != null && runOnlyCategories.Contains(category, StringComparer.OrdinalIgnoreCase);
    }

    private bool IsIgnoreCategory(string? category)
    {
        var ignoreCategories = _pipelineOptions.Value.IgnoreCategories?.ToArray();

        if (ignoreCategories is not { Length: > 0 })
        {
            return false;
        }

        return category != null && ignoreCategories.Contains(category, StringComparer.OrdinalIgnoreCase);
    }

    private async Task<(bool IsRunnable, SkipDecision? SkipDecision)> IsRunnableCondition(
        IModule module,
        CancellationToken cancellationToken,
        bool useFreshAttributes)
    {
        var moduleType = module.GetType();
        var pipelineContext = _pipelineContextProvider.GetModuleContext();
        var attributes = useFreshAttributes
            ? CreateConditionAttributes(moduleType)
            : GetConditionAttributes(moduleType);
        return await EvaluateConditions(
            attributes,
            pipelineContext,
            _executionLocationContext.ShouldDeferCapabilityConditions,
            cancellationToken,
            conditionGroupType => _executionLocationContext.IsConditionGroupSatisfied(
                module,
                conditionGroupType),
            conditionGroupType => _executionLocationContext.MarkConditionGroupSatisfied(
                module,
                conditionGroupType)).ConfigureAwait(false);
    }

    private ConditionAttributes GetConditionAttributes(Type moduleType)
    {
        return _conditionAttributes.GetOrAdd(
            moduleType,
            static type => new Lazy<ConditionAttributes>(
                () => CreateConditionAttributes(type),
                LazyThreadSafetyMode.ExecutionAndPublication)).Value;
    }

    private static ConditionAttributes CreateConditionAttributes(Type moduleType)
    {
        var attributes = moduleType.GetCustomAttributes(inherit: true);
        return Partition(attributes.OfType<RunConditionAttribute>().ToArray());
    }

    /// <summary>
    /// Splits condition attributes into skip attributes, run requirements that must each hold, and run
    /// attributes evaluated as alternatives: grouped attributes and the built-in any-of attributes.
    /// </summary>
    private static ConditionAttributes Partition(
        RunConditionAttribute[] attributes,
        bool hasDeferredSkip = false,
        bool hasDeferredAll = false,
        bool hasDeferredAny = false,
        bool hasDeferredGroupedAny = false) =>
        new(
            attributes.Where(static attribute => attribute.Intent == ConditionIntent.Skip).ToArray(),
            attributes.Where(static attribute => attribute.Intent == ConditionIntent.Run && !IsAlternative(attribute)).ToArray(),
            attributes.Where(static attribute => attribute.Intent == ConditionIntent.Run && IsAlternative(attribute)).ToArray(),
            hasDeferredSkip,
            hasDeferredAll,
            hasDeferredAny,
            hasDeferredGroupedAny);

    private static bool IsAlternative(RunConditionAttribute attribute) =>
        attribute.GroupKey is not null || attribute.Logic == ConditionLogic.Any;

    private static ConditionAttributes CreatePlanningConditionAttributes(Type moduleType)
    {
        var conditionData = CustomAttributeMetadata.GetApplicable(
            moduleType,
            static type => typeof(RunConditionAttribute).IsAssignableFrom(type));
        var planningAttributes = conditionData
            .Where(data => IsPlanningConditionAttribute(data.AttributeType))
            .Select(CustomAttributeMetadata.Create<RunConditionAttribute>)
            .ToArray();
        var deferredTypes = conditionData
            .Select(static data => data.AttributeType)
            .Where(static type => !IsPlanningConditionAttribute(type))
            .ToArray();

        // Deferred attributes are not constructed, so only built-in attributes reveal their intent and
        // logic. Any other deferred attribute may skip, require, or be a grouped alternative.
        return Partition(
            planningAttributes,
            deferredTypes.Any(static type => BuiltInConditionAttributes.GetIntent(type) is null or ConditionIntent.Skip),
            deferredTypes.Any(static type => !BuiltInConditionAttributes.IsBuiltIn(type)
                                             || BuiltInConditionAttributes.GetRunLogic(type) == ConditionLogic.All),
            deferredTypes.Any(static type => !BuiltInConditionAttributes.IsBuiltIn(type)
                                             || BuiltInConditionAttributes.GetRunLogic(type) == ConditionLogic.Any),
            deferredTypes.Any(static type => !BuiltInConditionAttributes.IsBuiltIn(type)));
    }

    internal static bool IsPlanningConditionAttribute(RunConditionAttribute attribute)
        => IsPlanningConditionAttribute(attribute.GetType());

    private static bool IsPlanningConditionAttribute(Type attributeType)
    {
        if (typeof(IPlanningSafe).IsAssignableFrom(attributeType))
        {
            return true;
        }

        return BuiltInConditionAttributes.IsBuiltIn(attributeType)
               && BuiltInConditionAttributes.GetConditionTypes(attributeType).All(static type =>
                   typeof(IPlanningSafe).IsAssignableFrom(type));
    }

    private static async Task<PlanningConditionResult> EvaluatePlanningConditions(
        ConditionAttributes attributes,
        IPipelineContext pipelineContext,
        bool shouldDeferCapabilityConditions,
        CancellationToken cancellationToken)
    {
        var skipEvaluation = await EvaluateSkipPlanningConditions(
                attributes.Skip,
                pipelineContext,
                attributes.HasDeferredSkip,
                cancellationToken)
            .ConfigureAwait(false);
        if (skipEvaluation.Result is not null)
        {
            return skipEvaluation.Result;
        }

        var allEvaluation = await EvaluateAllPlanningConditions(
                attributes.All,
                pipelineContext,
                shouldDeferCapabilityConditions,
                attributes.HasDeferredAll,
                cancellationToken)
            .ConfigureAwait(false);
        if (allEvaluation.Result is not null)
        {
            return allEvaluation.Result;
        }

        var anyEvaluation = await EvaluateAnyPlanningConditions(
                attributes.Any,
                pipelineContext,
                shouldDeferCapabilityConditions,
                attributes.HasDeferredAny,
                attributes.HasDeferredGroupedAny,
                cancellationToken)
            .ConfigureAwait(false);
        return anyEvaluation.Result ?? new PlanningConditionResult(
            false,
            null,
            skipEvaluation.IsResolved
            && allEvaluation.IsResolved
            && anyEvaluation.IsResolved);
    }

    private static async Task<PlanningConditionEvaluation> EvaluateSkipPlanningConditions(
        IEnumerable<RunConditionAttribute> attributes,
        IPipelineContext pipelineContext,
        bool hasDeferredConditions,
        CancellationToken cancellationToken)
    {
        var isResolved = !hasDeferredConditions;
        foreach (var attribute in attributes)
        {
            if (!IsPlanningConditionAttribute(attribute))
            {
                isResolved = false;
                continue;
            }

            cancellationToken.ThrowIfCancellationRequested();
            if (await attribute.EvaluateAsync(pipelineContext, cancellationToken).ConfigureAwait(false))
            {
                return new PlanningConditionEvaluation(
                    PlanningSkip($"SkipIf<{attribute.ConditionNames}> returned true"),
                    IsResolved: true);
            }
        }

        return new PlanningConditionEvaluation(null, isResolved);
    }

    private static async Task<PlanningConditionEvaluation> EvaluateAllPlanningConditions(
        IEnumerable<RunConditionAttribute> attributes,
        IPipelineContext pipelineContext,
        bool shouldDeferCapabilityConditions,
        bool hasDeferredConditions,
        CancellationToken cancellationToken)
    {
        var isResolved = !hasDeferredConditions;
        var allConditions = attributes.ToArray();
        var deferRequiredCapabilityConditions = shouldDeferCapabilityConditions
                                                 && CapabilityConditions.HasRoutableRequirement(allConditions);
        if (deferRequiredCapabilityConditions)
        {
            isResolved = false;
        }

        foreach (var attribute in allConditions)
        {
            if (deferRequiredCapabilityConditions
                && CapabilityConditions.GetRoute(attribute) is not null)
            {
                continue;
            }

            if (!IsPlanningConditionAttribute(attribute))
            {
                isResolved = false;
                continue;
            }

            cancellationToken.ThrowIfCancellationRequested();
            if (!await attribute.EvaluateAsync(pipelineContext, cancellationToken).ConfigureAwait(false))
            {
                return new PlanningConditionEvaluation(
                    PlanningSkip($"RunIf<{attribute.ConditionNames}> not satisfied"),
                    IsResolved: true);
            }
        }

        return new PlanningConditionEvaluation(null, isResolved);
    }

    private static async Task<PlanningConditionEvaluation> EvaluateAnyPlanningConditions(
        IReadOnlyCollection<RunConditionAttribute> attributes,
        IPipelineContext pipelineContext,
        bool shouldDeferCapabilityConditions,
        bool hasDeferredConditions,
        bool hasDeferredGroupedConditions,
        CancellationToken cancellationToken)
    {
        var isResolved = !hasDeferredConditions;
        foreach (var attribute in attributes.Where(static attribute =>
                     attribute.GroupKey is null))
        {
            if (ShouldDeferCapabilityCondition(attribute, shouldDeferCapabilityConditions))
            {
                if (await AnyConditionMatches(
                        CapabilityConditions.GetLocalAlternatives(attribute),
                        pipelineContext,
                        cancellationToken)
                    .ConfigureAwait(false))
                {
                    continue;
                }

                isResolved = false;
                continue;
            }

            var evaluation = await EvaluateSingleAnyPlanningCondition(
                    attribute,
                    pipelineContext,
                    cancellationToken)
                .ConfigureAwait(false);
            if (evaluation.Result is not null)
            {
                return evaluation;
            }
        }

        var evaluatedGroups = new HashSet<Type>();
        foreach (var groupKey in attributes.Select(static attribute => attribute.GroupKey).OfType<Type>())
        {
            if (!evaluatedGroups.Add(groupKey))
            {
                continue;
            }

            var evaluation = await EvaluateGroupedPlanningConditions(
                    attributes,
                    groupKey,
                    pipelineContext,
                    shouldDeferCapabilityConditions,
                    hasDeferredGroupedConditions,
                    cancellationToken)
                .ConfigureAwait(false);
            if (evaluation.Result is not null)
            {
                return evaluation;
            }

            isResolved &= evaluation.IsResolved;
        }

        return new PlanningConditionEvaluation(null, isResolved);
    }

    private static async Task<PlanningConditionEvaluation> EvaluateSingleAnyPlanningCondition(
        RunConditionAttribute attribute,
        IPipelineContext pipelineContext,
        CancellationToken cancellationToken)
    {
        if (!IsPlanningConditionAttribute(attribute))
        {
            return new PlanningConditionEvaluation(null, IsResolved: false);
        }

        cancellationToken.ThrowIfCancellationRequested();
        var matches = await attribute.EvaluateAsync(pipelineContext, cancellationToken)
            .ConfigureAwait(false);
        return matches
            ? new PlanningConditionEvaluation(null, IsResolved: true)
            : new PlanningConditionEvaluation(
                PlanningSkip($"RunIfAny<{attribute.ConditionNames}> not satisfied"),
                IsResolved: true);
    }

    private static async Task<PlanningConditionEvaluation> EvaluateGroupedPlanningConditions(
        IEnumerable<RunConditionAttribute> attributes,
        Type groupType,
        IPipelineContext pipelineContext,
        bool shouldDeferCapabilityConditions,
        bool hasDeferredGroupedConditions,
        CancellationToken cancellationToken)
    {
        var alternatives = attributes
            .Where(candidate => candidate.GroupKey == groupType)
            .ToArray();
        var planningAlternatives = alternatives
            .Where(attribute => !ShouldDeferCapabilityCondition(attribute, shouldDeferCapabilityConditions))
            .Where(IsPlanningConditionAttribute)
            .ToArray();
        if (await AnyConditionMatches(
                planningAlternatives,
                pipelineContext,
                cancellationToken)
            .ConfigureAwait(false))
        {
            return new PlanningConditionEvaluation(null, IsResolved: true);
        }

        if (hasDeferredGroupedConditions
            || planningAlternatives.Length != alternatives.Length)
        {
            return new PlanningConditionEvaluation(null, IsResolved: false);
        }

        return new PlanningConditionEvaluation(
            PlanningSkip(
                $"No grouped run conditions were met: {string.Join(", ", alternatives.Select(x => x.ConditionNames))}"),
            IsResolved: true);
    }

    private static PlanningConditionResult PlanningSkip(string reason) =>
        new(true, SkipDecision.Skip(reason), IsResolved: true);

    private readonly record struct PlanningConditionEvaluation(
        PlanningConditionResult? Result,
        bool IsResolved);

    private static async Task<(bool IsRunnable, SkipDecision? SkipDecision)> EvaluateConditions(
        ConditionAttributes attributes,
        IPipelineContext pipelineContext,
        bool shouldDeferCapabilityConditions,
        CancellationToken cancellationToken,
        Func<Type, bool>? isLocallySatisfiedConditionGroup = null,
        Action<Type>? locallySatisfiedConditionGroup = null)
    {
        var skipDecision = await EvaluateSkipConditions(
            attributes.Skip,
            pipelineContext,
            cancellationToken).ConfigureAwait(false);
        skipDecision ??= await EvaluateAllConditions(
            attributes.All,
            pipelineContext,
            shouldDeferCapabilityConditions,
            cancellationToken).ConfigureAwait(false);
        skipDecision ??= await EvaluateAnyConditions(
            attributes.Any,
            pipelineContext,
            shouldDeferCapabilityConditions,
            cancellationToken,
            isLocallySatisfiedConditionGroup,
            locallySatisfiedConditionGroup).ConfigureAwait(false);

        return skipDecision is null ? (true, null) : (false, skipDecision);
    }

    private static async Task<SkipDecision?> EvaluateSkipConditions(
        IEnumerable<RunConditionAttribute> attributes,
        IPipelineContext pipelineContext,
        CancellationToken cancellationToken)
    {
        foreach (var attribute in attributes)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (await attribute.EvaluateAsync(pipelineContext, cancellationToken).ConfigureAwait(false))
            {
                return SkipDecision.Skip($"SkipIf<{attribute.ConditionNames}> returned true");
            }
        }

        return null;
    }

    private static async Task<SkipDecision?> EvaluateAllConditions(
        IEnumerable<RunConditionAttribute> attributes,
        IPipelineContext pipelineContext,
        bool shouldDeferCapabilityConditions,
        CancellationToken cancellationToken)
    {
        var allConditions = attributes.ToArray();
        var deferRequiredCapabilityConditions = shouldDeferCapabilityConditions
                                                 && CapabilityConditions.HasRoutableRequirement(allConditions);

        foreach (var attribute in allConditions)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (deferRequiredCapabilityConditions
                && CapabilityConditions.GetRoute(attribute) is not null)
            {
                continue;
            }

            if (!await attribute.EvaluateAsync(pipelineContext, cancellationToken).ConfigureAwait(false))
            {
                return SkipDecision.Skip(
                    $"RunIf<{attribute.ConditionNames}> not satisfied");
            }
        }

        return null;
    }

    private static async Task<SkipDecision?> EvaluateAnyConditions(
        IReadOnlyList<RunConditionAttribute> attributes,
        IPipelineContext pipelineContext,
        bool shouldDeferCapabilityConditions,
        CancellationToken cancellationToken,
        Func<Type, bool>? isLocallySatisfiedConditionGroup,
        Action<Type>? locallySatisfiedConditionGroup)
    {
        var evaluatedGroups = new HashSet<Type>();

        foreach (var attribute in attributes)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (attribute.GroupKey is not { } groupKey)
            {
                var skipDecision = await EvaluateUngroupedAnyCondition(
                        attribute,
                        pipelineContext,
                        shouldDeferCapabilityConditions,
                        cancellationToken,
                        isLocallySatisfiedConditionGroup,
                        locallySatisfiedConditionGroup)
                    .ConfigureAwait(false);
                if (skipDecision is not null)
                {
                    return skipDecision;
                }

                continue;
            }

            if (!evaluatedGroups.Add(groupKey))
            {
                continue;
            }

            if (isLocallySatisfiedConditionGroup?.Invoke(groupKey) == true)
            {
                continue;
            }

            var alternatives = attributes
                .Where(candidate => candidate.GroupKey == groupKey)
                .ToArray();

            var localAlternatives = alternatives
                .Where(attribute =>
                    !ShouldDeferCapabilityCondition(attribute, shouldDeferCapabilityConditions))
                .ToArray();
            if (await AnyConditionMatches(
                    localAlternatives,
                    pipelineContext,
                    cancellationToken)
                .ConfigureAwait(false))
            {
                if (shouldDeferCapabilityConditions && localAlternatives.Length != alternatives.Length)
                {
                    locallySatisfiedConditionGroup?.Invoke(groupKey);
                }

                continue;
            }

            if (localAlternatives.Length != alternatives.Length)
            {
                continue;
            }

            return SkipDecision.Skip(
                $"No grouped run conditions were met: {string.Join(", ", alternatives.Select(x => x.ConditionNames))}");
        }

        return null;
    }

    private static async Task<SkipDecision?> EvaluateUngroupedAnyCondition(
        RunConditionAttribute attribute,
        IPipelineContext pipelineContext,
        bool shouldDeferCapabilityConditions,
        CancellationToken cancellationToken,
        Func<Type, bool>? isLocallySatisfiedConditionGroup,
        Action<Type>? locallySatisfiedConditionGroup)
    {
        if (isLocallySatisfiedConditionGroup?.Invoke(attribute.GetType()) == true)
        {
            return null;
        }

        if (!ShouldDeferCapabilityCondition(attribute, shouldDeferCapabilityConditions))
        {
            return await attribute.EvaluateAsync(pipelineContext, cancellationToken).ConfigureAwait(false)
                ? null
                : SkipDecision.Skip($"RunIfAny<{attribute.ConditionNames}> not satisfied");
        }

        if (await AnyConditionMatches(
                CapabilityConditions.GetLocalAlternatives(attribute),
                pipelineContext,
                cancellationToken)
            .ConfigureAwait(false))
        {
            locallySatisfiedConditionGroup?.Invoke(attribute.GetType());
        }

        return null;
    }

    private static bool ShouldDeferCapabilityCondition(
        RunConditionAttribute attribute,
        bool shouldDeferCapabilityConditions) =>
        shouldDeferCapabilityConditions && CapabilityConditions.IsRoutable(attribute);

    private static async Task<bool> AnyConditionMatches(
        IEnumerable<RunConditionAttribute> alternatives,
        IPipelineContext pipelineContext,
        CancellationToken cancellationToken)
    {
        foreach (var alternative in alternatives)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (await alternative.EvaluateAsync(pipelineContext, cancellationToken).ConfigureAwait(false))
            {
                return true;
            }
        }

        return false;
    }

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2067",
        Justification = "Condition types come from RunIfAny<T...> generic arguments with a new() constraint preserving public constructors.")]
    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2072",
        Justification = "Condition types come from RunIfAny<T...> generic arguments with a new() constraint preserving public constructors.")]
    private static async Task<bool> AnyConditionMatches(
        IEnumerable<Type> conditionTypes,
        IPipelineContext pipelineContext,
        CancellationToken cancellationToken)
    {
        foreach (var conditionType in conditionTypes)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var condition = (IRunCondition) Activator.CreateInstance(conditionType)!;
            if (await condition.EvaluateAsync(pipelineContext, cancellationToken).ConfigureAwait(false))
            {
                return true;
            }
        }

        return false;
    }

    private sealed class ConditionEvaluation
    {
        public SemaphoreSlim Gate { get; } = new(1, 1);

        public bool HasResult { get; set; }

        public (bool ShouldIgnore, SkipDecision? SkipDecision) Result { get; set; }
    }

    private sealed record ConditionAttributes(
        RunConditionAttribute[] Skip,
        RunConditionAttribute[] All,
        RunConditionAttribute[] Any,
        bool HasDeferredSkip = false,
        bool HasDeferredAll = false,
        bool HasDeferredAny = false,
        bool HasDeferredGroupedAny = false);
}
