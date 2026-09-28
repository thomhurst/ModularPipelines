namespace ModularPipelines;

/// <summary>
/// Marks an extension as safe to construct and evaluate while planning the dependency graph.
/// </summary>
/// <remarks>
/// <para>
/// Planning builds dry-run plans and exported dependency graphs without executing modules. Only
/// extensions marked with this interface are constructed or evaluated during planning; the rest are
/// deferred until pipeline execution, and the planned result reports them as unresolved.
/// </para>
/// <para>The marker applies to:</para>
/// <list type="bullet">
/// <item><see cref="IRunCondition"/> implementations, including those used by <see cref="RunIfAttribute{T}"/>,
/// <see cref="RunIfAnyAttribute{T1,T2}"/>, <see cref="SkipIfAttribute{T}"/>, and <see cref="ConditionGroup"/>;</item>
/// <item>stateful condition attributes derived from <see cref="RunConditionAttribute"/>;</item>
/// <item>dependency selector attributes derived from <see cref="DependsOnBaseAttribute"/>;</item>
/// <item><see cref="Events.IModuleRegistrationHandler"/> attributes.</item>
/// </list>
/// <para>
/// Implement it only when construction and evaluation are deterministic, idempotent, free of
/// observable side effects, and do not perform blocking work or remote I/O.
/// </para>
/// </remarks>
public interface IPlanningSafe;
