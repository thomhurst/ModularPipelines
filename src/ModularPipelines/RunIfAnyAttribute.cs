using ModularPipelines.Context;

namespace ModularPipelines;

/// <summary>
/// Specifies that at least one condition must be satisfied for the module to run (OR logic).
/// </summary>
/// <remarks>
/// <para>
/// When multiple conditions are specified, at least one must return true for the module to run.
/// If all conditions return false, the module is skipped.
/// </para>
/// <para>
/// Multiple <c>[RunIfAny]</c> attributes on a module are combined with AND logic between them.
/// </para>
/// </remarks>
/// <typeparam name="T">The condition type.</typeparam>
/// <example>
/// <code>
/// [RunIfAny&lt;OnLinux, OnMacOS&gt;]
/// public class UnixModule : Module&lt;None&gt;
/// {
///     // Runs on Linux OR macOS, skipped on Windows
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class RunIfAnyAttribute<T> : RunIfAnyAttribute
    where T : IRunCondition, new()
{
    /// <inheritdoc />
    public override Task<bool> EvaluateAsync(IPipelineContext context) =>
        EvaluateAsync(context, CancellationToken.None);

    /// <inheritdoc />
    public override Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken) =>
        RunConditionEvaluator.EvaluateAnyAsync([static () => new T()], context, cancellationToken);

    /// <inheritdoc />
    public override string ConditionNames => typeof(T).Name;
}

/// <summary>
/// Specifies that at least one condition must be satisfied for the module to run (OR logic).
/// </summary>
/// <typeparam name="T1">The first condition type.</typeparam>
/// <typeparam name="T2">The second condition type.</typeparam>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class RunIfAnyAttribute<T1, T2> : RunIfAnyAttribute
    where T1 : IRunCondition, new()
    where T2 : IRunCondition, new()
{
    /// <inheritdoc />
    public override Task<bool> EvaluateAsync(IPipelineContext context) =>
        EvaluateAsync(context, CancellationToken.None);

    /// <inheritdoc />
    public override Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken) =>
        RunConditionEvaluator.EvaluateAnyAsync([static () => new T1(), static () => new T2()], context, cancellationToken);

    /// <inheritdoc />
    public override string ConditionNames => $"{typeof(T1).Name}, {typeof(T2).Name}";
}

/// <summary>
/// Specifies that at least one condition must be satisfied for the module to run (OR logic).
/// </summary>
/// <typeparam name="T1">The first condition type.</typeparam>
/// <typeparam name="T2">The second condition type.</typeparam>
/// <typeparam name="T3">The third condition type.</typeparam>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class RunIfAnyAttribute<T1, T2, T3> : RunIfAnyAttribute
    where T1 : IRunCondition, new()
    where T2 : IRunCondition, new()
    where T3 : IRunCondition, new()
{
    /// <inheritdoc />
    public override Task<bool> EvaluateAsync(IPipelineContext context) =>
        EvaluateAsync(context, CancellationToken.None);

    /// <inheritdoc />
    public override Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken) =>
        RunConditionEvaluator.EvaluateAnyAsync(
            [static () => new T1(), static () => new T2(), static () => new T3()],
            context,
            cancellationToken);

    /// <inheritdoc />
    public override string ConditionNames => $"{typeof(T1).Name}, {typeof(T2).Name}, {typeof(T3).Name}";
}

/// <summary>
/// Specifies that at least one condition must be satisfied for the module to run (OR logic).
/// </summary>
/// <typeparam name="T1">The first condition type.</typeparam>
/// <typeparam name="T2">The second condition type.</typeparam>
/// <typeparam name="T3">The third condition type.</typeparam>
/// <typeparam name="T4">The fourth condition type.</typeparam>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class RunIfAnyAttribute<T1, T2, T3, T4> : RunIfAnyAttribute
    where T1 : IRunCondition, new()
    where T2 : IRunCondition, new()
    where T3 : IRunCondition, new()
    where T4 : IRunCondition, new()
{
    /// <inheritdoc />
    public override Task<bool> EvaluateAsync(IPipelineContext context) =>
        EvaluateAsync(context, CancellationToken.None);

    /// <inheritdoc />
    public override Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken) =>
        RunConditionEvaluator.EvaluateAnyAsync(
            [static () => new T1(), static () => new T2(), static () => new T3(), static () => new T4()],
            context,
            cancellationToken);

    /// <inheritdoc />
    public override string ConditionNames => $"{typeof(T1).Name}, {typeof(T2).Name}, {typeof(T3).Name}, {typeof(T4).Name}";
}
