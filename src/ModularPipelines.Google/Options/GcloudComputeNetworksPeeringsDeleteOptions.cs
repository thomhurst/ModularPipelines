using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "networks", "peerings", "delete")]
public record GcloudComputeNetworksPeeringsDeleteOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--network")] string Network
) : GcloudOptions;