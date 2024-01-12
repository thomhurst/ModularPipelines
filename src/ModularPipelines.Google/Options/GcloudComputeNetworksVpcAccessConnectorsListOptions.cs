using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "networks", "vpc-access", "connectors", "list")]
public record GcloudComputeNetworksVpcAccessConnectorsListOptions(
[property: CommandSwitch("--region")] string Region
) : GcloudOptions;