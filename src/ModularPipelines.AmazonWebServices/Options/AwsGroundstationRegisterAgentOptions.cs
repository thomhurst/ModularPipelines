using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("groundstation", "register-agent")]
public record AwsGroundstationRegisterAgentOptions(
[property: CommandSwitch("--agent-details")] string AgentDetails,
[property: CommandSwitch("--discovery-data")] string DiscoveryData
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}