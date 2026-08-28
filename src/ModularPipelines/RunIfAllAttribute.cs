using ModularPipelines.Context;

namespace ModularPipelines;

/// <summary>
/// Specifies that all conditions must be satisfied for the module to run (AND logic).
/// </summary>
/// <typeparam name="T1">The first condition type.</typeparam>
/// <typeparam name="T2">The second condition type.</typeparam>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class RunIfAllAttribute<T1, T2> : RunIfAllAttribute
    where T1 : IRunCondition, new()
    where T2 : IRunCondition, new()
{
    /// <inheritdoc />
    public override Task<bool> EvaluateAsync(IPipelineContext context) =>
        EvaluateAsync(context, CancellationToken.None);

    /// <inheritdoc />
    public override Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken) =>
        RunConditionEvaluator.EvaluateAllAsync([static () => new T1(), static () => new T2()], context, cancellationToken);

    /// <inheritdoc />
    public override string ConditionNames => $"{typeof(T1).Name}, {typeof(T2).Name}";
}

/// <summary>
/// Specifies that all conditions must be satisfied for the module to run (AND logic).
/// </summary>
/// <typeparam name="T1">The first condition type.</typeparam>
/// <typeparam name="T2">The second condition type.</typeparam>
/// <typeparam name="T3">The third condition type.</typeparam>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class RunIfAllAttribute<T1, T2, T3> : RunIfAllAttribute
    where T1 : IRunCondition, new()
    where T2 : IRunCondition, new()
    where T3 : IRunCondition, new()
{
    /// <inheritdoc />
    public override Task<bool> EvaluateAsync(IPipelineContext context) =>
        EvaluateAsync(context, CancellationToken.None);

    /// <inheritdoc />
    public override Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken) =>
        RunConditionEvaluator.EvaluateAllAsync(
            [static () => new T1(), static () => new T2(), static () => new T3()],
            context,
            cancellationToken);

    /// <inheritdoc />
    public override string ConditionNames => $"{typeof(T1).Name}, {typeof(T2).Name}, {typeof(T3).Name}";
}

/// <summary>
/// Specifies that all conditions must be satisfied for the module to run (AND logic).
/// </summary>
/// <typeparam name="T1">The first condition type.</typeparam>
/// <typeparam name="T2">The second condition type.</typeparam>
/// <typeparam name="T3">The third condition type.</typeparam>
/// <typeparam name="T4">The fourth condition type.</typeparam>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class RunIfAllAttribute<T1, T2, T3, T4> : RunIfAllAttribute
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
        RunConditionEvaluator.EvaluateAllAsync(
            [static () => new T1(), static () => new T2(), static () => new T3(), static () => new T4()],
            context,
            cancellationToken);

    /// <inheritdoc />
    public override string ConditionNames => $"{typeof(T1).Name}, {typeof(T2).Name}, {typeof(T3).Name}, {typeof(T4).Name}";
}
