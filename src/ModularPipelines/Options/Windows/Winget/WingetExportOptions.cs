using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Windows.Winget;

/// <summary>
/// Options for the 'winget export' command.
/// Exports a list of installed packages.
/// </summary>
[ExcludeFromCodeCoverage]
public record WingetExportOptions : WingetOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WingetExportOptions"/> class
    /// with the specified output file.
    /// </summary>
    /// <param name="output">The output file path.</param>
    public WingetExportOptions(string output)
    {
        Output = output;
    }

    /// <summary>
    /// Gets the command name.
    /// </summary>
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string CommandName { get; } = "export";

    /// <summary>
    /// Gets the output file path.
    /// </summary>
    [CliFlag("--output")]
    public virtual string Output { get; }

    /// <summary>
    /// Gets or sets the source to filter by.
    /// </summary>
    [CliFlag("--source")]
    public virtual string? Source { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to include version numbers.
    /// </summary>
    [CliFlag("--include-versions")]
    public virtual bool? IncludeVersions { get; set; }

    /// <summary>
    /// Gets or sets the header to use for REST source operations.
    /// </summary>
    [CliFlag("--header")]
    public virtual string? Header { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to accept source agreements.
    /// </summary>
    [CliFlag("--accept-source-agreements")]
    public virtual bool? AcceptSourceAgreements { get; set; }
}
