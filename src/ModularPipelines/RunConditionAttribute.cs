using ModularPipelines.Context;

namespace ModularPipelines;

/// <summary>
/// Base class for condition attributes, including stateful attributes that take their condition
/// state through constructor arguments.
/// </summary>
/// <remarks>
/// <para>
/// <see cref="Intent"/> decides what a satisfied condition means. A module is skipped when any
/// <see cref="ConditionIntent.Skip"/> attribute is satisfied. Otherwise it runs only when every
/// <see cref="ConditionIntent.Run"/> requirement is satisfied, where each ungrouped attribute is
/// one requirement and all run attributes sharing a <see cref="GroupKey"/> form one requirement
/// that any of them satisfies. Skip attributes are evaluated before run attributes.
/// </para>
/// <para>
/// Implement <see cref="IPlanningSafe"/> on the derived attribute when construction and evaluation
/// are side-effect free and perform no blocking work or remote I/O, so dry-run planning can resolve it.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public sealed class RunOnBranchAttribute(string branch) : RunConditionAttribute(ConditionIntent.Run)
/// {
///     public override Type? GroupKey =&gt; typeof(RunOnBranchAttribute);
///
///     public override string ConditionNames =&gt; $"RunOnBranch({branch})";
///
///     public override async Task&lt;bool&gt; EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken)
///     {
///         var info = await context.Tools.Git.Information.GetInfoAsync(cancellationToken);
///         return info?.BranchName == branch;
///     }
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public abstract class RunConditionAttribute : Attribute, IRunCondition
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RunConditionAttribute"/> class.
    /// </summary>
    /// <param name="intent">Whether a satisfied condition runs or skips the module.</param>
    protected RunConditionAttribute(ConditionIntent intent)
    {
        if (intent is not (ConditionIntent.Run or ConditionIntent.Skip))
        {
            throw new ArgumentOutOfRangeException(nameof(intent), intent, "Unknown condition intent.");
        }

        Intent = intent;
    }

    /// <summary>
    /// Gets whether a satisfied condition runs or skips the module.
    /// </summary>
    public ConditionIntent Intent { get; }

    /// <summary>
    /// Gets the key that groups alternative <see cref="ConditionIntent.Run"/> attributes, or
    /// <see langword="null"/> when this attribute is a requirement of its own.
    /// </summary>
    /// <remarks>
    /// Run attributes on one module that share a key are alternatives: the module may run when any of
    /// them is satisfied. The key is ignored for <see cref="ConditionIntent.Skip"/> attributes, because
    /// any satisfied skip attribute already skips the module.
    /// </remarks>
    public virtual Type? GroupKey => null;

    /// <summary>
    /// Gets a human-readable description of the condition for skip reasons and logs.
    /// </summary>
    public virtual string ConditionNames => GetType().Name;

    /// <summary>
    /// Gets how the built-in generic attributes combine their condition type arguments.
    /// </summary>
    internal virtual ConditionLogic Logic => ConditionLogic.All;

    /// <inheritdoc />
    public abstract Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken);
}

/// <summary>
/// Describes the built-in generic condition attributes by type, so planning can reason about them
/// without constructing arbitrary attributes.
/// </summary>
internal static class BuiltInConditionAttributes
{
    private static readonly HashSet<Type> RunIfDefinitions =
    [
        typeof(RunIfAttribute<>),
        typeof(RunIfAttribute<,>),
        typeof(RunIfAttribute<,,>),
        typeof(RunIfAttribute<,,,>),
    ];

    private static readonly HashSet<Type> RunIfAnyDefinitions =
    [
        typeof(RunIfAnyAttribute<,>),
        typeof(RunIfAnyAttribute<,,>),
        typeof(RunIfAnyAttribute<,,,>),
    ];

    private static readonly HashSet<Type> SkipIfDefinitions =
    [
        typeof(SkipIfAttribute<>),
        typeof(SkipIfAttribute<,>),
        typeof(SkipIfAttribute<,,>),
        typeof(SkipIfAttribute<,,,>),
    ];

    /// <summary>
    /// Returns whether the type is a closed built-in generic condition attribute.
    /// </summary>
    public static bool IsBuiltIn(Type attributeType) => GetDefinitionKind(attributeType) is not null;

    /// <summary>
    /// Returns the condition types of a built-in generic attribute, or an empty array for other attributes.
    /// </summary>
    public static Type[] GetConditionTypes(Type attributeType) =>
        IsBuiltIn(attributeType) ? attributeType.GetGenericArguments() : [];

    /// <summary>
    /// Returns the condition types an attribute is composed of: the type arguments of a built-in generic
    /// attribute, or of a custom generic attribute whose type arguments are all constructible run conditions
    /// (for example <c>GroupedOperatingSystemAttribute&lt;OnLinux&gt;</c>). Other attributes have none.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.UnconditionalSuppressMessage(
        "Trimming",
        "IL2070",
        Justification = "Only checks for a parameterless constructor; attributes whose condition types are trimmed are treated as having none.")]
    public static Type[] GetMemberConditionTypes(Type attributeType)
    {
        if (!attributeType.IsConstructedGenericType)
        {
            return [];
        }

        var typeArguments = attributeType.GetGenericArguments();
        return IsBuiltIn(attributeType)
               || typeArguments.All(static type =>
                   typeof(IRunCondition).IsAssignableFrom(type)
                   && !type.IsAbstract
                   && type.GetConstructor(Type.EmptyTypes) is not null)
            ? typeArguments
            : [];
    }

    /// <summary>
    /// Returns the intent of a built-in generic attribute, or <see langword="null"/> when the intent is only
    /// known after construction.
    /// </summary>
    public static ConditionIntent? GetIntent(Type attributeType) => GetDefinitionKind(attributeType) switch
    {
        DefinitionKind.SkipIf => ConditionIntent.Skip,
        DefinitionKind.RunIf or DefinitionKind.RunIfAny => ConditionIntent.Run,
        _ => null,
    };

    /// <summary>
    /// Returns how a built-in generic run attribute combines its condition types, or <see langword="null"/>
    /// for other attributes.
    /// </summary>
    public static ConditionLogic? GetRunLogic(Type attributeType) => GetDefinitionKind(attributeType) switch
    {
        DefinitionKind.RunIf => ConditionLogic.All,
        DefinitionKind.RunIfAny => ConditionLogic.Any,
        _ => null,
    };

    private static DefinitionKind? GetDefinitionKind(Type attributeType)
    {
        if (!attributeType.IsConstructedGenericType)
        {
            return null;
        }

        var definition = attributeType.GetGenericTypeDefinition();
        if (RunIfDefinitions.Contains(definition))
        {
            return DefinitionKind.RunIf;
        }

        if (RunIfAnyDefinitions.Contains(definition))
        {
            return DefinitionKind.RunIfAny;
        }

        return SkipIfDefinitions.Contains(definition) ? DefinitionKind.SkipIf : null;
    }

    private enum DefinitionKind
    {
        RunIf,
        RunIfAny,
        SkipIf,
    }
}
