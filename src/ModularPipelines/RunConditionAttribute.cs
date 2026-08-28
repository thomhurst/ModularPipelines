using ModularPipelines.Context;

namespace ModularPipelines;

/// <summary>
/// Base class for stateful run-condition attributes.
/// </summary>
/// <remarks>
/// Derive from one of the intent-specific base classes and accept condition state through
/// the derived attribute constructor.
/// </remarks>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public abstract class RunConditionAttribute : Attribute, IConditionAttribute
{
    /// <inheritdoc />
    public abstract ConditionLogic Logic { get; }

    /// <inheritdoc />
    public virtual string ConditionNames => GetType().Name;

    /// <inheritdoc />
    public abstract Task<bool> EvaluateAsync(IPipelineContext context);

    /// <inheritdoc />
    public virtual Task<bool> EvaluateAsync(
        IPipelineContext context,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return EvaluateAsync(context);
    }
}

/// <summary>
/// Base class for stateful attributes that skip a module when their condition is satisfied.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public abstract class SkipIfAttribute : RunConditionAttribute
{
    /// <inheritdoc />
    public sealed override ConditionLogic Logic => ConditionLogic.Skip;
}

/// <summary>
/// Base class for stateful attributes that require one condition for a module to run.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public abstract class RunIfAttribute : RunConditionAttribute
{
    /// <inheritdoc />
    public sealed override ConditionLogic Logic => ConditionLogic.All;
}

/// <summary>
/// Base class for stateful attributes that require their condition for a module to run.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public abstract class RunIfAllAttribute : RunConditionAttribute
{
    /// <inheritdoc />
    public sealed override ConditionLogic Logic => ConditionLogic.All;
}

/// <summary>
/// Base class for stateful alternative attributes that allow a module to run.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public abstract class RunIfAnyAttribute : RunConditionAttribute
{
    /// <inheritdoc />
    public sealed override ConditionLogic Logic => ConditionLogic.Any;
}
