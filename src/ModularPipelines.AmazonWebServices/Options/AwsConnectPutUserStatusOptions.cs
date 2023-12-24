using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "put-user-status")]
public record AwsConnectPutUserStatusOptions(
[property: CommandSwitch("--user-id")] string UserId,
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--agent-status-id")] string AgentStatusId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}