using ModularPipelines.Conditions;
using ModularPipelines.Context;

namespace ModularPipelines.Attributes;

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
public sealed class SkipIfAttribute<T> : Attribute, IConditionAttribute
    where T : IRunCondition, new()
{
    /// <inheritdoc />
    public ConditionLogic Logic => ConditionLogic.Skip;

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
/// Specifies that the module should be skipped if any condition is satisfied.
/// </summary>
/// <typeparam name="T1">The first condition type.</typeparam>
/// <typeparam name="T2">The second condition type.</typeparam>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public sealed class SkipIfAttribute<T1, T2> : Attribute, IConditionAttribute
    where T1 : IRunCondition, new()
    where T2 : IRunCondition, new()
{
    /// <inheritdoc />
    public ConditionLogic Logic => ConditionLogic.Skip;

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
