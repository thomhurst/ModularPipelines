using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Windows.Chocolatey;

/// <summary>
/// Options for the 'choco list' command.
/// Lists locally installed packages.
/// </summary>
[ExcludeFromCodeCoverage]
public record ChocolateyListOptions : ChocolateyOptions
{
    /// <summary>
    /// Gets the command name.
    /// </summary>
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string CommandName { get; } = "list";

    /// <summary>
    /// Gets or sets the source to search (defaults to local).
    /// </summary>
    [CliFlag("--source")]
    public virtual string? Source { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to include pre-release versions.
    /// </summary>
    [CliFlag("--pre")]
    public virtual bool? PreRelease { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to include programs (Programs and Features).
    /// </summary>
    [CliFlag("--include-programs")]
    public virtual bool? IncludePrograms { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to show all versions.
    /// </summary>
    [CliFlag("--all-versions")]
    public virtual bool? AllVersions { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to only show exact matches.
    /// </summary>
    [CliFlag("--exact")]
    public virtual bool? Exact { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to show by ID-only.
    /// </summary>
    [CliFlag("--id-only")]
    public virtual bool? IdOnly { get; set; }

    /// <summary>
    /// Gets or sets the page number for results.
    /// </summary>
    [CliFlag("--page")]
    public virtual int? Page { get; set; }

    /// <summary>
    /// Gets or sets the page size for results.
    /// </summary>
    [CliFlag("--page-size")]
    public virtual int? PageSize { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to order by popularity.
    /// </summary>
    [CliFlag("--order-by-popularity")]
    public virtual bool? OrderByPopularity { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to include configured sources in operation.
    /// </summary>
    [CliFlag("--use-windows-services")]
    public virtual bool? UseWindowsServices { get; set; }
}
