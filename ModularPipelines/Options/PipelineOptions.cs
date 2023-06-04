namespace ModularPipelines.Options;

public record PipelineOptions
{
    public bool StopOnFirstException { get; set; } = true;
    public string[]? RunOnlyCategories { get; set; }
    public string[]? IgnoreCategories { get; set; }
    public ModuleLoggerOptions LoggerOptions { get; } = new();
}