using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("services", "vpc-peerings", "operations", "wait")]
public record GcloudServicesVpcPeeringsOperationsWaitOptions(
[property: CommandSwitch("--name")] string Name
) : GcloudOptions;