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
    /// Evaluates the formula. <paramref name="atomValue"/> returns an ordinary condition's value, or
    /// <c>null</c> to leave it out: it then neither constrains an AND nor satisfies an OR, which projects
    /// the formula onto its capability conditions.
    /// </summary>
    public abstract FormulaValue Evaluate(Func<ConditionAtom, bool?> atomValue);

    /// <summary>
    /// Builds the formula of a module's non-skip condition attributes, or <c>null</c> when they impose
    /// nothing. Grouped attributes become one OR per group. Groups the master already satisfied are left out.
    /// </summary>
    public static ConditionFormula? ForModule(
        IEnumerable<IConditionAttribute> attributes,
        Func<Type, bool>? isConditionGroupSatisfied = null)
    {
        var attributeArray = attributes.ToArray();
        var formulas = new List<ConditionFormula>();
        foreach (var attribute in attributeArray.Where(static attribute => attribute is not IGroupedConditionAttribute))
        {
            if (isConditionGroupSatisfied?.Invoke(attribute.GetType()) != true
                && ForAttribute(attribute) is { } formula)
            {
                formulas.Add(formula);
            }
        }

        foreach (var alternatives in attributeArray
                     .OfType<IGroupedConditionAttribute>()
                     .GroupBy(static attribute => attribute.ConditionGroupType))
        {
            if (isConditionGroupSatisfied?.Invoke(alternatives.Key) != true)
            {
                formulas.Add(new OrFormula([.. alternatives.Select(ForAlternative)]));
            }
        }

        return formulas.Count == 0 ? null : new AndFormula(formulas);
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

        public override FormulaValue Evaluate(Func<ConditionAtom, bool?> atomValue) =>
            FormulaValue.Of(CapabilityRequirement.AllOf(capability));
    }

    private sealed class AndFormula(IReadOnlyList<ConditionFormula> members) : ConditionFormula
    {
        public override IEnumerable<ConditionAtom> Atoms => members.SelectMany(static member => member.Atoms);

        public override FormulaValue Evaluate(Func<ConditionAtom, bool?> atomValue)
        {
            var values = members.Select(member => member.Evaluate(atomValue)).ToArray();
            if (values.Any(static value => value.Kind == FormulaValueKind.False))
            {
                return FormulaValue.False;
            }

            var requirements = values
                .Where(static value => value.Kind == FormulaValueKind.Requirement)
                .Select(static value => value.Requirement!)
                .ToArray();
            if (requirements.Length > 0)
            {
                return FormulaValue.Of(requirements.Aggregate(CapabilityRequirement.None, static (left, right) => left.And(right)));
            }

            return values.All(static value => value.Kind == FormulaValueKind.True)
                ? FormulaValue.True
                : FormulaValue.Unknown;
        }
    }

    private sealed class OrFormula(IReadOnlyList<ConditionFormula> members) : ConditionFormula
    {
        public override IEnumerable<ConditionAtom> Atoms => members.SelectMany(static member => member.Atoms);

        public override FormulaValue Evaluate(Func<ConditionAtom, bool?> atomValue)
        {
            var values = members.Select(member => member.Evaluate(atomValue)).ToArray();
            if (values.Any(static value => value.Kind == FormulaValueKind.True))
            {
                return FormulaValue.True;
            }

            var requirements = values
                .Where(static value => value.Kind == FormulaValueKind.Requirement)
                .Select(static value => value.Requirement!)
                .ToArray();
            if (requirements.Length > 0)
            {
                return FormulaValue.Of(requirements.Aggregate(static (left, right) => left.Or(right)));
            }

            return values.All(static value => value.Kind == FormulaValueKind.False)
                ? FormulaValue.False
                : FormulaValue.Unknown;
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

    public Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken) =>
        evaluate(context, cancellationToken);

    public override FormulaValue Evaluate(Func<ConditionAtom, bool?> atomValue) =>
        atomValue(this) switch
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
}
