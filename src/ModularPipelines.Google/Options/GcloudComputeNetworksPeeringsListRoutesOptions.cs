using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "networks", "peerings", "list-routes")]
public record GcloudComputeNetworksPeeringsListRoutesOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--direction")] string Direction,
[property: CommandSwitch("--network")] string Network,
[property: CommandSwitch("--region")] string Region
) : GcloudOptions;