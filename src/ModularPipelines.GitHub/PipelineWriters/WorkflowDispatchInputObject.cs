namespace ModularPipelines.GitHub.PipelineWriters;

public record WorkflowDispatchInputObject
{
    public string Description { get; init; } = null!;

    public string Type { get; init; } = "string";
    
    public bool Required { get; init; }
    
    public bool? Default { get; init; }
}