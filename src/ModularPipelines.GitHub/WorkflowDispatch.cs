namespace ModularPipelines.GitHub;

public record WorkflowDispatch
{
    public IDictionary<string, WorkflowDispatchInputObject> Inputs { get; init; } =
        new Dictionary<string, WorkflowDispatchInputObject>();
}