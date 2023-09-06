using System.Diagnostics.CodeAnalysis;

namespace ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public record PipelineOptions
{
    public ExecutionMode ExecutionMode { get; set; } = ExecutionMode.StopOnFirstException;
    public string[]? RunOnlyCategories { get; set; }
    public string[]? IgnoreCategories { get; set; }
    public bool ShowProgressInConsole { get; set; } = true;
    public ModuleLoggerOptions LoggerOptions { get; } = new();
}
