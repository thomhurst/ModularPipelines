using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("groundstation", "register-agent")]
public record AwsGroundstationRegisterAgentOptions(
[property: CliOption("--agent-details")] string AgentDetails,
[property: CliOption("--discovery-data")] string DiscoveryData
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}