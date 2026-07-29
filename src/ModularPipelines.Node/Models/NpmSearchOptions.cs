using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Node.Models;

[ExcludeFromCodeCoverage]
[CliCommand("search", "terms")]
public record NpmSearchOptions
    (
        [property: CliArgument(Placement = ArgumentPlacement.BeforeOptions)] string Value
        ) : NpmOptions
{
    [CliFlag("--long")]
    public virtual bool? Long { get; set; }

    [CliFlag("--json")]
    public virtual bool? Json { get; set; }

    [CliOption("--color")]
    public virtual string? Color { get; set; }

    [CliFlag("--parseable")]
    public virtual bool? Parseable { get; set; }

    [CliFlag("--description")]
    public virtual bool? Description { get; set; }

    [CliOption("--searchopts")]
    public virtual string? Searchopts { get; set; }

    [CliOption("--searchexclude")]
    public virtual string? Searchexclude { get; set; }

    [CliOption("--registry")]
    public virtual Uri? Registry { get; set; }

    [CliFlag("--prefer-online")]
    public virtual bool? PreferOnline { get; set; }

    [CliFlag("--prefer-offline")]
    public virtual bool? PreferOffline { get; set; }

    [CliFlag("--offline")]
    public virtual bool? Offline { get; set; }
}