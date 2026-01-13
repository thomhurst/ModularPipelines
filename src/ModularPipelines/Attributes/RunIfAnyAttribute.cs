using ModularPipelines.Conditions;
using ModularPipelines.Context;

namespace ModularPipelines.Attributes;

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
public sealed class RunIfAnyAttribute<T> : Attribute, IConditionAttribute
    where T : IRunCondition, new()
{
    /// <inheritdoc />
    public ConditionLogic Logic => ConditionLogic.Any;

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
/// Specifies that at least one condition must be satisfied for the module to run (OR logic).
/// </summary>
/// <typeparam name="T1">The first condition type.</typeparam>
/// <typeparam name="T2">The second condition type.</typeparam>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class RunIfAnyAttribute<T1, T2> : Attribute, IConditionAttribute
    where T1 : IRunCondition, new()
    where T2 : IRunCondition, new()
{
    /// <inheritdoc />
    public ConditionLogic Logic => ConditionLogic.Any;

    /// <inheritdoc />
    public async Task<bool> EvaluateAsync(IPipelineHookContext context)
    {
        if (await new T1().EvaluateAsync(context))
        {
            return true;
        }

        return await new T2().EvaluateAsync(context);
    }

    /// <inheritdoc />
    public string ConditionNames => $"{typeof(T1).Name}, {typeof(T2).Name}";
}

/// <summary>
/// Specifies that at least one condition must be satisfied for the module to run (OR logic).
/// </summary>
/// <typeparam name="T1">The first condition type.</typeparam>
/// <typeparam name="T2">The second condition type.</typeparam>
/// <typeparam name="T3">The third condition type.</typeparam>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class RunIfAnyAttribute<T1, T2, T3> : Attribute, IConditionAttribute
    where T1 : IRunCondition, new()
    where T2 : IRunCondition, new()
    where T3 : IRunCondition, new()
{
    /// <inheritdoc />
    public ConditionLogic Logic => ConditionLogic.Any;

    /// <inheritdoc />
    public async Task<bool> EvaluateAsync(IPipelineHookContext context)
    {
        if (await new T1().EvaluateAsync(context))
        {
            return true;
        }

        if (await new T2().EvaluateAsync(context))
        {
            return true;
        }

        return await new T3().EvaluateAsync(context);
    }

    /// <inheritdoc />
    public string ConditionNames => $"{typeof(T1).Name}, {typeof(T2).Name}, {typeof(T3).Name}";
}

/// <summary>
/// Specifies that at least one condition must be satisfied for the module to run (OR logic).
/// </summary>
/// <typeparam name="T1">The first condition type.</typeparam>
/// <typeparam name="T2">The second condition type.</typeparam>
/// <typeparam name="T3">The third condition type.</typeparam>
/// <typeparam name="T4">The fourth condition type.</typeparam>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class RunIfAnyAttribute<T1, T2, T3, T4> : Attribute, IConditionAttribute
    where T1 : IRunCondition, new()
    where T2 : IRunCondition, new()
    where T3 : IRunCondition, new()
    where T4 : IRunCondition, new()
{
    /// <inheritdoc />
    public ConditionLogic Logic => ConditionLogic.Any;

    /// <inheritdoc />
    public async Task<bool> EvaluateAsync(IPipelineHookContext context)
    {
        if (await new T1().EvaluateAsync(context))
        {
            return true;
        }

        if (await new T2().EvaluateAsync(context))
        {
            return true;
        }

        if (await new T3().EvaluateAsync(context))
        {
            return true;
        }

        return await new T4().EvaluateAsync(context);
    }

    /// <inheritdoc />
    public string ConditionNames => $"{typeof(T1).Name}, {typeof(T2).Name}, {typeof(T3).Name}, {typeof(T4).Name}";
}
