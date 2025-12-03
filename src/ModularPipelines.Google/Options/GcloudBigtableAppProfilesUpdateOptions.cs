using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bigtable", "app-profiles", "update")]
public record GcloudBigtableAppProfilesUpdateOptions(
[property: CliArgument] string AppProfile,
[property: CliArgument] string Instance
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--force")]
    public bool? Force { get; set; }

    [CliOption("--priority")]
    public string? Priority { get; set; }

    [CliFlag("--route-any")]
    public bool? RouteAny { get; set; }

    [CliOption("--restrict-to")]
    public string[]? RestrictTo { get; set; }

    [CliOption("--route-to")]
    public string? RouteTo { get; set; }

    [CliFlag("--transactional-writes")]
    public bool? TransactionalWrites { get; set; }
}