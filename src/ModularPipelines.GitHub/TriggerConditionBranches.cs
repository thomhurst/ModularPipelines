namespace ModularPipelines.GitHub;

public record TriggerConditionBranches
{
    public IEnumerable<string>? Branches { get; init; }
}