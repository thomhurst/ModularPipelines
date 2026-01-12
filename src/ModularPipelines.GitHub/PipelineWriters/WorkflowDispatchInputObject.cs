namespace ModularPipelines.GitHub.PipelineWriters;

public record WorkflowDispatchInputObject
{
    public required string Description { get; init; }

    public string Type { get; init; } = "string";

    public bool Required { get; init; }

    public bool? Default { get; init; }
}