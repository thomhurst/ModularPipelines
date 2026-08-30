using ModularPipelines.Context;

namespace ModularPipelines;

/// <summary>
/// Specifies that the module should be skipped if the condition is satisfied.
/// </summary>
/// <remarks>
/// <para>
/// If the condition returns true, the module is skipped.
/// This is useful for excluding certain scenarios (e.g., skip on Dependabot PRs).
/// </para>
/// <para>
/// <c>[SkipIf]</c> attributes are evaluated first, before <c>[RunIfAll]</c> and <c>[RunIfAny]</c>.
/// </para>
/// </remarks>
/// <typeparam name="T">The condition type. If it returns true, the module is skipped.</typeparam>
/// <example>
/// <code>
/// [SkipIf&lt;IsDependabot&gt;]
/// public class ReleaseModule : Module&lt;None&gt;
/// {
///     // Skipped when running on Dependabot PRs
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class SkipIfAttribute<T> : SkipIfAttribute
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
/// Specifies that the module should be skipped if any condition is satisfied.
/// </summary>
/// <typeparam name="T1">The first condition type.</typeparam>
/// <typeparam name="T2">The second condition type.</typeparam>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class SkipIfAttribute<T1, T2> : SkipIfAttribute
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
/// Specifies that the module should be skipped if any condition is satisfied.
/// </summary>
/// <typeparam name="T1">The first condition type.</typeparam>
/// <typeparam name="T2">The second condition type.</typeparam>
/// <typeparam name="T3">The third condition type.</typeparam>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class SkipIfAttribute<T1, T2, T3> : SkipIfAttribute
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
/// Specifies that the module should be skipped if any condition is satisfied.
/// </summary>
/// <typeparam name="T1">The first condition type.</typeparam>
/// <typeparam name="T2">The second condition type.</typeparam>
/// <typeparam name="T3">The third condition type.</typeparam>
/// <typeparam name="T4">The fourth condition type.</typeparam>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class SkipIfAttribute<T1, T2, T3, T4> : SkipIfAttribute
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
    public override string ConditionNames =>
        $"{typeof(T1).Name}, {typeof(T2).Name}, {typeof(T3).Name}, {typeof(T4).Name}";
}
