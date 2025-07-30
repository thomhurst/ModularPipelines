using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Enums;
using Spectre.Console;

namespace ModularPipelines.Options;

/// <summary>
/// Configuration options for pipeline execution behavior.
/// </summary>
[ExcludeFromCodeCoverage]
public record PipelineOptions
{
    /// <summary>
    /// Gets or sets the execution mode that determines how the pipeline handles failures.
    /// </summary>
    public ExecutionMode ExecutionMode { get; set; } = ExecutionMode.StopOnFirstException;

    /// <summary>
    /// Gets or sets the collection of module categories to run exclusively. If specified, only modules in these categories will run.
    /// </summary>
    public ICollection<string>? RunOnlyCategories { get; set; }

    /// <summary>
    /// Gets or sets the collection of module categories to ignore during execution.
    /// </summary>
    public ICollection<string>? IgnoreCategories { get; set; }

    private bool _showProgressInConsole = AnsiConsole.Profile.Capabilities.Interactive;

    /// <summary>
    /// Gets or sets a value indicating whether to show progress information in the console.
    /// </summary>
    public bool ShowProgressInConsole
    {
        get => _showProgressInConsole;
        set => AnsiConsole.Profile.Capabilities.Interactive = _showProgressInConsole = value;
    }

    /// <summary>
    /// Gets or sets a value indicating whether to print execution results to the console.
    /// </summary>
    public bool PrintResults { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether to print the ModularPipelines logo.
    /// </summary>
    public bool PrintLogo { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether to print module dependency chains.
    /// </summary>
    public bool PrintDependencyChains { get; set; } = true;

    /// <summary>
    /// Gets or sets the default number of retry attempts for failed operations.
    /// </summary>
    public int DefaultRetryCount { get; set; }

    /// <summary>
    /// Gets or sets the default command logging level for all commands.
    /// </summary>
    public CommandLogging DefaultCommandLogging { get; set; } = CommandLogging.Default;
}