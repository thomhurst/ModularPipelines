using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Distributed;
using ModularPipelines.Engine;

namespace ModularPipelines;

/// <summary>
/// A boolean formula over a module's run conditions. Leaves are capability conditions, which a worker
/// satisfies by advertising the capability, and ordinary conditions (<see cref="ConditionAtom"/>), which
/// something must evaluate. Evaluating the formula with known values for some ordinary conditions yields
/// the capability requirement a worker must meet for the module's conditions to be able to hold.
/// </summary>
internal abstract class ConditionFormula
{
    /// <summary>
    /// Gets the ordinary conditions in this formula.
    /// </summary>
    public abstract IEnumerable<ConditionAtom> Atoms { get; }

    /// <summary>
    /// Gets the capabilities named by this formula's capability conditions.
    /// </summary>
    public abstract IEnumerable<Capability> Capabilities { get; }

    /// <summary>
    /// Evaluates the formula. <paramref name="atomValue"/> returns an ordinary condition's value, or
    /// <c>null</c> to leave it out: it then neither constrains an AND nor satisfies an OR, which projects
    /// the formula onto its capability conditions.
    /// </summary>
    public FormulaValue Evaluate(Func<ConditionAtom, bool?> atomValue) =>
        EvaluateAsync(atom => Task.FromResult(atomValue(atom))).GetAwaiter().GetResult();

    /// <summary>
    /// Evaluates the formula left to right with the short-circuiting of run condition evaluation: an AND
    /// stops at its first false member and an OR at its first true member, so later conditions are not
    /// evaluated.
    /// </summary>
    public abstract Task<FormulaValue> EvaluateAsync(Func<ConditionAtom, Task<bool?>> atomValue);

    /// <summary>
    /// Builds one formula per condition group of a module's non-skip condition attributes, keyed by the
    /// group type the master marks as satisfied. Ungrouped attributes are their own group.
    /// </summary>
    public static IEnumerable<(Type ConditionGroupType, ConditionFormula Formula)> ForConditionGroups(
        IEnumerable<IConditionAttribute> attributes)
    {
        var attributeArray = attributes.ToArray();
        foreach (var attribute in attributeArray.Where(static attribute => attribute is not IGroupedConditionAttribute))
        {
            if (ForAttribute(attribute) is { } formula)
            {
                yield return (attribute.GetType(), formula);
            }
        }

        foreach (var alternatives in attributeArray
                     .OfType<IGroupedConditionAttribute>()
                     .GroupBy(static attribute => attribute.ConditionGroupType))
        {
            yield return (alternatives.Key, new OrFormula([.. alternatives.Select(ForAlternative)]));
        }
    }

    /// <summary>
    /// Builds the formula of a module's non-skip condition attributes, or <c>null</c> when they impose
    /// nothing. Groups the master already satisfied are left out.
    /// </summary>
    public static ConditionFormula? ForModule(
        IEnumerable<IConditionAttribute> attributes,
        Func<Type, bool>? isConditionGroupSatisfied = null)
    {
        var formulas = ForConditionGroups(attributes)
            .Where(group => isConditionGroupSatisfied?.Invoke(group.ConditionGroupType) != true)
            .Select(static group => group.Formula)
            .ToArray();
        return formulas.Length == 0 ? null : new AndFormula(formulas);
    }

    /// <summary>
    /// Builds the formula of the condition groups that contain a capability condition, which are the only
    /// groups that affect routing, or <c>null</c> when there are none.
    /// </summary>
    public static ConditionFormula? ForRouting(IEnumerable<IConditionAttribute> attributes)
    {
        var formulas = ForConditionGroups(attributes)
            .Select(static group => group.Formula)
            .Where(static formula => formula.Capabilities.Any())
            .ToArray();
        return formulas.Length == 0 ? null : new AndFormula(formulas);
    }

    /// <summary>
    /// Builds the formula of one condition attribute, or <c>null</c> for skip conditions, which never
    /// require a capability.
    /// </summary>
    public static ConditionFormula? ForAttribute(IConditionAttribute attribute)
    {
        if (attribute.Logic is not (ConditionLogic.All or ConditionLogic.Any))
        {
            return null;
        }

        var conditionTypes = attribute.GetType().GetGenericArguments();
        if (conditionTypes.Length == 0)
        {
            return AttributeAtom(attribute);
        }

        var members = conditionTypes.Select(ForConditionType).ToArray();
        return attribute.Logic == ConditionLogic.All ? new AndFormula(members) : new OrFormula(members);
    }

    private static ConditionFormula ForAlternative(IConditionAttribute attribute) =>
        ForAttribute(attribute) ?? AttributeAtom(attribute);

    private static ConditionAtom AttributeAtom(IConditionAttribute attribute) =>
        new(
            ModuleConditionHandler.IsPlanningConditionAttribute(attribute),
            attribute.EvaluateAsync);

    [UnconditionalSuppressMessage(
        "Trimming",
        "IL2067",
        Justification = "Condition types come from RunIf<T>, RunIfAll<T...>, or RunIfAny<T...> generic arguments, whose new() constraints preserve a public parameterless constructor.")]
    private static ConditionFormula ForConditionType(Type conditionType)
    {
        var isPlanning = typeof(IPlanningRunCondition).IsAssignableFrom(conditionType);

        // Only planning-safe conditions may be constructed on the master.
        if (isPlanning
            && (typeof(ICapabilityCondition).IsAssignableFrom(conditionType)
                || typeof(ConditionGroup).IsAssignableFrom(conditionType))
            && Activator.CreateInstance(conditionType) is IRunCondition condition)
        {
            return ForCondition(condition, isPlanning: true);
        }

        return new ConditionAtom(
            isPlanning,
            (context, cancellationToken) =>
                ((IRunCondition) Activator.CreateInstance(conditionType)!).EvaluateAsync(context, cancellationToken));
    }

    private static ConditionFormula ForCondition(IRunCondition condition, bool isPlanning)
    {
        if (condition is ICapabilityCondition capabilityCondition)
        {
            return new CapabilityFormula(capabilityCondition.Capability);
        }

        if (condition is ConditionGroup { Logic: ConditionLogic.All or ConditionLogic.Any, Conditions.Count: > 0 } group
            && condition is IPlanningRunCondition)
        {
            // A planning-safe group vouches for evaluating its members during planning.
            var members = group.Conditions.Select(member => ForCondition(member, isPlanning: true)).ToArray();
            return group.Logic == ConditionLogic.All ? new AndFormula(members) : new OrFormula(members);
        }

        return new ConditionAtom(isPlanning, condition.EvaluateAsync);
    }

    private sealed class CapabilityFormula(Capability capability) : ConditionFormula
    {
        public override IEnumerable<ConditionAtom> Atoms => [];

        public override IEnumerable<Capability> Capabilities => [capability];

        public override Task<FormulaValue> EvaluateAsync(Func<ConditionAtom, Task<bool?>> atomValue) =>
            Task.FromResult(FormulaValue.Of(CapabilityRequirement.AllOf(capability)));
    }

    private sealed class AndFormula(IReadOnlyList<ConditionFormula> members) : ConditionFormula
    {
        public override IEnumerable<ConditionAtom> Atoms => members.SelectMany(static member => member.Atoms);

        public override IEnumerable<Capability> Capabilities => members.SelectMany(static member => member.Capabilities);

        public override async Task<FormulaValue> EvaluateAsync(Func<ConditionAtom, Task<bool?>> atomValue)
        {
            var result = FormulaValue.True;
            foreach (var member in members)
            {
                result = FormulaValue.And(result, await member.EvaluateAsync(atomValue).ConfigureAwait(false));
                if (result.Kind == FormulaValueKind.False)
                {
                    break;
                }
            }

            return result;
        }
    }

    private sealed class OrFormula(IReadOnlyList<ConditionFormula> members) : ConditionFormula
    {
        public override IEnumerable<ConditionAtom> Atoms => members.SelectMany(static member => member.Atoms);

        public override IEnumerable<Capability> Capabilities => members.SelectMany(static member => member.Capabilities);

        public override async Task<FormulaValue> EvaluateAsync(Func<ConditionAtom, Task<bool?>> atomValue)
        {
            var result = FormulaValue.False;
            foreach (var member in members)
            {
                result = FormulaValue.Or(result, await member.EvaluateAsync(atomValue).ConfigureAwait(false));
                if (result.Kind == FormulaValueKind.True)
                {
                    break;
                }
            }

            return result;
        }
    }
}

/// <summary>
/// An ordinary run condition inside a <see cref="ConditionFormula"/>. Atoms compare by reference, so
/// one formula instance maps each condition occurrence to one value.
/// </summary>
internal sealed class ConditionAtom(
    bool isPlanning,
    Func<IPipelineContext, CancellationToken, Task<bool>> evaluate) : ConditionFormula
{
    /// <summary>
    /// Gets whether the condition is safe to evaluate on the master during planning.
    /// </summary>
    public bool IsPlanning { get; } = isPlanning;

    public override IEnumerable<ConditionAtom> Atoms => [this];

    public override IEnumerable<Capability> Capabilities => [];

    public Task<bool> EvaluateConditionAsync(IPipelineContext context, CancellationToken cancellationToken) =>
        evaluate(context, cancellationToken);

    public override async Task<FormulaValue> EvaluateAsync(Func<ConditionAtom, Task<bool?>> atomValue) =>
        await atomValue(this).ConfigureAwait(false) switch
        {
            true => FormulaValue.True,
            false => FormulaValue.False,
            null => FormulaValue.Unknown,
        };
}

internal enum FormulaValueKind
{
    /// <summary>The conditions hold on every worker.</summary>
    True,

    /// <summary>The conditions hold on no worker.</summary>
    False,

    /// <summary>The value was left out and imposes nothing.</summary>
    Unknown,

    /// <summary>The conditions can hold only on workers that satisfy <see cref="FormulaValue.Requirement"/>.</summary>
    Requirement,
}

internal readonly record struct FormulaValue(FormulaValueKind Kind, CapabilityRequirement? Requirement)
{
    public static FormulaValue True { get; } = new(FormulaValueKind.True, null);

    public static FormulaValue False { get; } = new(FormulaValueKind.False, null);

    public static FormulaValue Unknown { get; } = new(FormulaValueKind.Unknown, null);

    /// <summary>
    /// Gets the capability requirement implied by this value, or <c>null</c> when no worker qualifies.
    /// </summary>
    public CapabilityRequirement? RequirementOrNone => Kind switch
    {
        FormulaValueKind.False => null,
        FormulaValueKind.Requirement => Requirement,
        _ => CapabilityRequirement.None,
    };

    public static FormulaValue Of(CapabilityRequirement requirement) =>
        requirement.IsSatisfiable ? new(FormulaValueKind.Requirement, requirement) : False;

    /// <summary>
    /// Combines two conjuncts. An unknown conjunct imposes nothing, but two unknowns stay unknown.
    /// </summary>
    public static FormulaValue And(FormulaValue left, FormulaValue right) => (left.Kind, right.Kind) switch
    {
        (FormulaValueKind.False, _) or (_, FormulaValueKind.False) => False,
        (FormulaValueKind.Requirement, FormulaValueKind.Requirement) => Of(left.Requirement!.And(right.Requirement!)),
        (FormulaValueKind.Requirement, _) => left,
        (_, FormulaValueKind.Requirement) => right,
        (FormulaValueKind.True, FormulaValueKind.True) => True,
        _ => Unknown,
    };

    /// <summary>
    /// Combines two disjuncts. An unknown disjunct satisfies nothing, but two unknowns stay unknown.
    /// </summary>
    public static FormulaValue Or(FormulaValue left, FormulaValue right) => (left.Kind, right.Kind) switch
    {
        (FormulaValueKind.True, _) or (_, FormulaValueKind.True) => True,
        (FormulaValueKind.Requirement, FormulaValueKind.Requirement) => Of(left.Requirement!.Or(right.Requirement!)),
        (FormulaValueKind.Requirement, _) => left,
        (_, FormulaValueKind.Requirement) => right,
        (FormulaValueKind.False, FormulaValueKind.False) => False,
        _ => Unknown,
    };
}
