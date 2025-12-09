using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "describe-agent-status")]
public record AwsConnectDescribeAgentStatusOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--agent-status-id")] string AgentStatusId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}