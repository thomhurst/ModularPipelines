using ModularPipelines.Conditions;
using ModularPipelines.Context;

namespace ModularPipelines.Attributes;

/// <summary>
/// Specifies that all conditions must be satisfied for the module to run (AND logic).
/// </summary>
/// <remarks>
/// <para>
/// When multiple conditions are specified, ALL must return true for the module to run.
/// If any condition returns false, the module is skipped.
/// </para>
/// <para>
/// Multiple <c>[RunIfAll]</c> attributes on a module are combined with AND logic between them.
/// </para>
/// </remarks>
/// <typeparam name="T">The condition type that must be satisfied.</typeparam>
/// <example>
/// <code>
/// [RunIfAll&lt;HasGitHubToken&gt;]
/// [RunIfAll&lt;IsCI&gt;]
/// public class PublishModule : Module&lt;None&gt;
/// {
///     // Only runs if BOTH HasGitHubToken AND IsCI return true
/// }
/// </code>
/// </example>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class RunIfAllAttribute<T> : Attribute, IConditionAttribute
    where T : IRunCondition, new()
{
    /// <inheritdoc />
    public ConditionLogic Logic => ConditionLogic.All;

    /// <inheritdoc />
    public async Task<bool> EvaluateAsync(IPipelineHookContext context)
    {
        var condition = new T();
        return await condition.EvaluateAsync(context);
    }

    /// <inheritdoc />
    public string ConditionNames => typeof(T).Name;
}

/// <summary>
/// Specifies that all conditions must be satisfied for the module to run (AND logic).
/// </summary>
/// <typeparam name="T1">The first condition type.</typeparam>
/// <typeparam name="T2">The second condition type.</typeparam>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class RunIfAllAttribute<T1, T2> : Attribute, IConditionAttribute
    where T1 : IRunCondition, new()
    where T2 : IRunCondition, new()
{
    /// <inheritdoc />
    public ConditionLogic Logic => ConditionLogic.All;

    /// <inheritdoc />
    public async Task<bool> EvaluateAsync(IPipelineHookContext context)
    {
        if (!await new T1().EvaluateAsync(context))
        {
            return false;
        }

        return await new T2().EvaluateAsync(context);
    }

    /// <inheritdoc />
    public string ConditionNames => $"{typeof(T1).Name}, {typeof(T2).Name}";
}

/// <summary>
/// Specifies that all conditions must be satisfied for the module to run (AND logic).
/// </summary>
/// <typeparam name="T1">The first condition type.</typeparam>
/// <typeparam name="T2">The second condition type.</typeparam>
/// <typeparam name="T3">The third condition type.</typeparam>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class RunIfAllAttribute<T1, T2, T3> : Attribute, IConditionAttribute
    where T1 : IRunCondition, new()
    where T2 : IRunCondition, new()
    where T3 : IRunCondition, new()
{
    /// <inheritdoc />
    public ConditionLogic Logic => ConditionLogic.All;

    /// <inheritdoc />
    public async Task<bool> EvaluateAsync(IPipelineHookContext context)
    {
        if (!await new T1().EvaluateAsync(context))
        {
            return false;
        }

        if (!await new T2().EvaluateAsync(context))
        {
            return false;
        }

        return await new T3().EvaluateAsync(context);
    }

    /// <inheritdoc />
    public string ConditionNames => $"{typeof(T1).Name}, {typeof(T2).Name}, {typeof(T3).Name}";
}

/// <summary>
/// Specifies that all conditions must be satisfied for the module to run (AND logic).
/// </summary>
/// <typeparam name="T1">The first condition type.</typeparam>
/// <typeparam name="T2">The second condition type.</typeparam>
/// <typeparam name="T3">The third condition type.</typeparam>
/// <typeparam name="T4">The fourth condition type.</typeparam>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class RunIfAllAttribute<T1, T2, T3, T4> : Attribute, IConditionAttribute
    where T1 : IRunCondition, new()
    where T2 : IRunCondition, new()
    where T3 : IRunCondition, new()
    where T4 : IRunCondition, new()
{
    /// <inheritdoc />
    public ConditionLogic Logic => ConditionLogic.All;

    /// <inheritdoc />
    public async Task<bool> EvaluateAsync(IPipelineHookContext context)
    {
        if (!await new T1().EvaluateAsync(context))
        {
            return false;
        }

        if (!await new T2().EvaluateAsync(context))
        {
            return false;
        }

        if (!await new T3().EvaluateAsync(context))
        {
            return false;
        }

        return await new T4().EvaluateAsync(context);
    }

    /// <inheritdoc />
    public string ConditionNames => $"{typeof(T1).Name}, {typeof(T2).Name}, {typeof(T3).Name}, {typeof(T4).Name}";
}
