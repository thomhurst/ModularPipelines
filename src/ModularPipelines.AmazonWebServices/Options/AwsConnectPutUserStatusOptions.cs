using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "put-user-status")]
public record AwsConnectPutUserStatusOptions(
[property: CliOption("--user-id")] string UserId,
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--agent-status-id")] string AgentStatusId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}