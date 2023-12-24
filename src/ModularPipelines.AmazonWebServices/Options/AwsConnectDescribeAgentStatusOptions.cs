using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "describe-agent-status")]
public record AwsConnectDescribeAgentStatusOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--agent-status-id")] string AgentStatusId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}