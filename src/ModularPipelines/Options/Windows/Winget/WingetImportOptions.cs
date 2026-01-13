using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Windows.Winget;

/// <summary>
/// Options for the 'winget import' command.
/// Imports packages from a file.
/// </summary>
[ExcludeFromCodeCoverage]
public record WingetImportOptions : WingetOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WingetImportOptions"/> class
    /// with the specified import file.
    /// </summary>
    /// <param name="importFile">The import file path.</param>
    public WingetImportOptions(string importFile)
    {
        ImportFile = importFile;
    }

    /// <summary>
    /// Gets the command name.
    /// </summary>
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string CommandName { get; } = "import";

    /// <summary>
    /// Gets the import file path.
    /// </summary>
    [CliFlag("--import-file")]
    public virtual string ImportFile { get; }

    /// <summary>
    /// Gets or sets a value indicating whether to ignore package versions.
    /// </summary>
    [CliFlag("--ignore-versions")]
    public virtual bool? IgnoreVersions { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to ignore unavailable packages.
    /// </summary>
    [CliFlag("--ignore-unavailable")]
    public virtual bool? IgnoreUnavailable { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to accept package agreements.
    /// </summary>
    [CliFlag("--accept-package-agreements")]
    public virtual bool? AcceptPackageAgreements { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to accept source agreements.
    /// </summary>
    [CliFlag("--accept-source-agreements")]
    public virtual bool? AcceptSourceAgreements { get; set; }

    /// <summary>
    /// Gets or sets the header to use for REST source operations.
    /// </summary>
    [CliFlag("--header")]
    public virtual string? Header { get; set; }
}
