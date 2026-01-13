using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Options.MacOS.Brew;

/// <summary>
/// Options for the 'brew search' command.
/// Searches for formulae and casks.
/// </summary>
[ExcludeFromCodeCoverage]
public record BrewSearchOptions : BrewOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BrewSearchOptions"/> class
    /// with the specified search text.
    /// </summary>
    /// <param name="text">The text to search for.</param>
    public BrewSearchOptions(string text)
    {
        Text = text;
    }

    /// <summary>
    /// Gets the command name.
    /// </summary>
    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public virtual string CommandName { get; } = "search";

    /// <summary>
    /// Gets the text to search for.
    /// </summary>
    [CliArgument(1, Placement = ArgumentPlacement.AfterOptions)]
    public virtual string Text { get; }

    /// <summary>
    /// Gets or sets a value indicating whether to search for casks.
    /// </summary>
    [CliFlag("--cask")]
    public virtual bool? Cask { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to search for formulae.
    /// </summary>
    [CliFlag("--formula")]
    public virtual bool? Formula { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to include descriptions in search.
    /// </summary>
    [CliFlag("--desc")]
    public virtual bool? Desc { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to search using extended regex.
    /// </summary>
    [CliFlag("--eval-all")]
    public virtual bool? EvalAll { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to limit output to open results.
    /// </summary>
    [CliFlag("--open")]
    public virtual bool? Open { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to limit output to closed results.
    /// </summary>
    [CliFlag("--closed")]
    public virtual bool? Closed { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether to search pull requests.
    /// </summary>
    [CliFlag("--pull-request")]
    public virtual bool? PullRequest { get; set; }
}
