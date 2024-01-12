using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bigtable", "app-profiles", "create")]
public record GcloudBigtableAppProfilesCreateOptions(
[property: PositionalArgument] string AppProfile,
[property: PositionalArgument] string Instance,
[property: BooleanCommandSwitch("--route-any")] bool RouteAny,
[property: CommandSwitch("--restrict-to")] string[] RestrictTo,
[property: CommandSwitch("--route-to")] string RouteTo,
[property: BooleanCommandSwitch("--transactional-writes")] bool TransactionalWrites
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [CommandSwitch("--priority")]
    public string? Priority { get; set; }
}