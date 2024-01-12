using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bigtable", "app-profiles", "update")]
public record GcloudBigtableAppProfilesUpdateOptions(
[property: PositionalArgument] string AppProfile,
[property: PositionalArgument] string Instance
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [CommandSwitch("--priority")]
    public string? Priority { get; set; }

    [BooleanCommandSwitch("--route-any")]
    public bool? RouteAny { get; set; }

    [CommandSwitch("--restrict-to")]
    public string[]? RestrictTo { get; set; }

    [CommandSwitch("--route-to")]
    public string? RouteTo { get; set; }

    [BooleanCommandSwitch("--transactional-writes")]
    public bool? TransactionalWrites { get; set; }
}