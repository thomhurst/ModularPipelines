namespace ModularPipelines.Options;

public class PipelineOptions
{
    public bool StopOnFirstException { get; set; }
    public string[]? RunOnlyCategories { get; set; }
    public string[]? IgnoreCategories { get; set; }
}