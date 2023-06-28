namespace ModularPipelines.Options;

public record PipelineOptions
{
    public ExecutionMode ExecutionMode { get; set; } = ExecutionMode.StopOnFirstException;
    public string[]? RunOnlyCategories { get; set; }
    public string[]? IgnoreCategories { get; set; }
    public ModuleLoggerOptions LoggerOptions { get; } = new();
}