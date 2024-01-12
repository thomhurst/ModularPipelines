using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "networks", "vpc-access", "operations", "list")]
public record GcloudComputeNetworksVpcAccessOperationsListOptions(
[property: CommandSwitch("--region")] string Region
) : GcloudOptions;