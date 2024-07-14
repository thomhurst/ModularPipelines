using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Enums;
using Spectre.Console;

namespace ModularPipelines.Options;

[ExcludeFromCodeCoverage]
public record PipelineOptions
{
    public ExecutionMode ExecutionMode { get; set; } = ExecutionMode.StopOnFirstException;

    public ICollection<string>? RunOnlyCategories { get; set; }

    public ICollection<string>? IgnoreCategories { get; set; }

    private bool _showProgressInConsole = AnsiConsole.Profile.Capabilities.Interactive;
    
    public bool ShowProgressInConsole
    {
        get => _showProgressInConsole;
        set => AnsiConsole.Profile.Capabilities.Interactive = _showProgressInConsole = value;
    }

    public bool PrintResults { get; set; } = true;

    public bool PrintLogo { get; set; } = true;

    public bool PrintDependencyChains { get; set; } = true;

    public int DefaultRetryCount { get; set; }

    public CommandLogging DefaultCommandLogging { get; set; } = CommandLogging.Default;
}