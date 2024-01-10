namespace ModularPipelines.GitHub.PipelineWriters;

public record TriggerConditionBranches
{
    public IEnumerable<string>? Branches { get; init; }
}