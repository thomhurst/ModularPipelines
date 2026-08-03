namespace ModularPipelines.Conditions;

/// <summary>
/// Marks a run condition as safe to evaluate while constructing a dependency-graph plan.
/// </summary>
/// <remarks>
/// Planning conditions must be side-effect free and must not perform blocking work or remote I/O.
/// Conditions without this marker remain unresolved until pipeline execution.
/// </remarks>
public interface IPlanningRunCondition : IRunCondition;
