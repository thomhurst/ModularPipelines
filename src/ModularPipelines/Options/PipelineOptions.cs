using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Enums;

namespace ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public record PipelineOptions
{
    public ExecutionMode ExecutionMode { get; set; } = ExecutionMode.StopOnFirstException;
    public ICollection<string>? RunOnlyCategories { get; set; }
    public ICollection<string>? IgnoreCategories { get; set; }
    public bool ShowProgressInConsole { get; set; } = true;
    public ModuleLoggerOptions LoggerOptions { get; set; } = new();
    public int DefaultRetryCount { get; set; }
    
    public CommandLogging DefaultCommandLogging { get; set; } =
        CommandLogging.Input | CommandLogging.Output | CommandLogging.Error;
}
