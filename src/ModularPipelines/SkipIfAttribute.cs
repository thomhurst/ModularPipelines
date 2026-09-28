using ModularPipelines.Context;

namespace ModularPipelines;

/// <summary>
/// Skips the module when the condition is satisfied.
/// </summary>
/// <typeparam name="T">The condition type.</typeparam>
/// <remarks>
/// Skip attributes are evaluated before run attributes.
/// </remarks>
/// <example>
/// <code>
/// [SkipIf&lt;IsDependabot&gt;]
/// public class ReleaseModule : Module&lt;None&gt;
/// {
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class SkipIfAttribute<T> : RunConditionAttribute
    where T : IRunCondition, new()
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SkipIfAttribute{T}"/> class.
    /// </summary>
    public SkipIfAttribute()
        : base(ConditionIntent.Skip)
    {
    }

    /// <inheritdoc />
    internal override ConditionLogic Logic => ConditionLogic.Any;

    /// <inheritdoc />
    public override string ConditionNames => $"{typeof(T).Name}";

    /// <inheritdoc />
    public override Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken) =>
        RunConditionEvaluator.EvaluateAnyAsync([static () => new T()], context, cancellationToken);
}

/// <summary>
/// Skips the module when any condition is satisfied.
/// </summary>
/// <typeparam name="T1">The first condition type.</typeparam>
/// <typeparam name="T2">The second condition type.</typeparam>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class SkipIfAttribute<T1, T2> : RunConditionAttribute
    where T1 : IRunCondition, new()
    where T2 : IRunCondition, new()
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SkipIfAttribute{T1,T2}"/> class.
    /// </summary>
    public SkipIfAttribute()
        : base(ConditionIntent.Skip)
    {
    }

    /// <inheritdoc />
    internal override ConditionLogic Logic => ConditionLogic.Any;

    /// <inheritdoc />
    public override string ConditionNames => $"{typeof(T1).Name}, {typeof(T2).Name}";

    /// <inheritdoc />
    public override Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken) =>
        RunConditionEvaluator.EvaluateAnyAsync([static () => new T1(), static () => new T2()], context, cancellationToken);
}

/// <summary>
/// Skips the module when any condition is satisfied.
/// </summary>
/// <typeparam name="T1">The first condition type.</typeparam>
/// <typeparam name="T2">The second condition type.</typeparam>
/// <typeparam name="T3">The third condition type.</typeparam>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class SkipIfAttribute<T1, T2, T3> : RunConditionAttribute
    where T1 : IRunCondition, new()
    where T2 : IRunCondition, new()
    where T3 : IRunCondition, new()
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SkipIfAttribute{T1,T2,T3}"/> class.
    /// </summary>
    public SkipIfAttribute()
        : base(ConditionIntent.Skip)
    {
    }

    /// <inheritdoc />
    internal override ConditionLogic Logic => ConditionLogic.Any;

    /// <inheritdoc />
    public override string ConditionNames => $"{typeof(T1).Name}, {typeof(T2).Name}, {typeof(T3).Name}";

    /// <inheritdoc />
    public override Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken) =>
        RunConditionEvaluator.EvaluateAnyAsync([static () => new T1(), static () => new T2(), static () => new T3()], context, cancellationToken);
}

/// <summary>
/// Skips the module when any condition is satisfied.
/// </summary>
/// <typeparam name="T1">The first condition type.</typeparam>
/// <typeparam name="T2">The second condition type.</typeparam>
/// <typeparam name="T3">The third condition type.</typeparam>
/// <typeparam name="T4">The fourth condition type.</typeparam>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class SkipIfAttribute<T1, T2, T3, T4> : RunConditionAttribute
    where T1 : IRunCondition, new()
    where T2 : IRunCondition, new()
    where T3 : IRunCondition, new()
    where T4 : IRunCondition, new()
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SkipIfAttribute{T1,T2,T3,T4}"/> class.
    /// </summary>
    public SkipIfAttribute()
        : base(ConditionIntent.Skip)
    {
    }

    /// <inheritdoc />
    internal override ConditionLogic Logic => ConditionLogic.Any;

    /// <inheritdoc />
    public override string ConditionNames => $"{typeof(T1).Name}, {typeof(T2).Name}, {typeof(T3).Name}, {typeof(T4).Name}";

    /// <inheritdoc />
    public override Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken) =>
        RunConditionEvaluator.EvaluateAnyAsync([static () => new T1(), static () => new T2(), static () => new T3(), static () => new T4()], context, cancellationToken);
}
