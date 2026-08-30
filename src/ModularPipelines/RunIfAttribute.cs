using ModularPipelines.Context;

namespace ModularPipelines;

/// <summary>
/// Specifies a condition that must be satisfied for the module to run.
/// </summary>
/// <typeparam name="T">The condition type that must be satisfied.</typeparam>
/// <example>
/// <code>
/// [RunIf&lt;OnCI&gt;]
/// public class PublishModule : Module&lt;None&gt;
/// {
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class RunIfAttribute<T> : RunIfAttribute
    where T : IRunCondition, new()
{
    /// <inheritdoc />
    public override Task<bool> EvaluateAsync(IPipelineContext context) =>
        EvaluateAsync(context, CancellationToken.None);

    /// <inheritdoc />
    public override Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return new T().EvaluateAsync(context, cancellationToken);
    }

    /// <inheritdoc />
    public override string ConditionNames => typeof(T).Name;
}
