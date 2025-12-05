using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bigtable", "app-profiles", "create")]
public record GcloudBigtableAppProfilesCreateOptions(
[property: CliArgument] string AppProfile,
[property: CliArgument] string Instance,
[property: CliFlag("--route-any")] bool RouteAny,
[property: CliOption("--restrict-to")] string[] RestrictTo,
[property: CliOption("--route-to")] string RouteTo,
[property: CliFlag("--transactional-writes")] bool TransactionalWrites
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--force")]
    public bool? Force { get; set; }

    [CliOption("--priority")]
    public string? Priority { get; set; }
}