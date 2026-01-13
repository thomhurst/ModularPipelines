using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.Windows.Chocolatey;

/// <summary>
/// Options for the 'choco search' command.
/// Searches remote packages.
/// </summary>
[ExcludeFromCodeCoverage]
public record ChocolateySearchOptions : ChocolateyOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChocolateySearchOptions"/> class
    /// with the specified search filter.
    /// </summary>
    /// <param name="filter">The search filter text.</param>
    public ChocolateySearchOptions(string filter)
    {
        Filter = filter;
    }

    /// <summary>
    /// Gets the command name.
    /// </summary>
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string CommandName { get; } = "search";

    /// <summary>
    /// Gets the search filter.
    /// </summary>
    [CliArgument(1, Placement = ArgumentPlacement.AfterOptions)]
    public virtual string Filter { get; }

    /// <summary>
    /// Gets or sets the source to search.
    /// </summary>
    [CliFlag("--source")]
    public virtual string? Source { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to include pre-release versions.
    /// </summary>
    [CliFlag("--pre")]
    public virtual bool? PreRelease { get; set; }

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
    /// Gets or sets a value indicating whether to include approved packages only.
    /// </summary>
    [CliFlag("--approved-only")]
    public virtual bool? ApprovedOnly { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to include packages that have been downloaded a certain number of times.
    /// </summary>
    [CliFlag("--download-cache-only")]
    public virtual bool? DownloadCacheOnly { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to not include packages that have been marked as not broken.
    /// </summary>
    [CliFlag("--not-broken")]
    public virtual bool? NotBroken { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to include detailed information.
    /// </summary>
    [CliFlag("--detailed")]
    public virtual bool? Detailed { get; set; }

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
    /// Gets or sets a value indicating whether to search by tags only.
    /// </summary>
    [CliFlag("--by-tag-only")]
    public virtual bool? ByTagOnly { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to search by ID starts with.
    /// </summary>
    [CliFlag("--by-id-only")]
    public virtual bool? ByIdOnly { get; set; }
}
