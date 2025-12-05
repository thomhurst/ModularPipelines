using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "update-agent-status")]
public record AwsConnectUpdateAgentStatusOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--agent-status-id")] string AgentStatusId
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--state")]
    public string? State { get; set; }

    [CliOption("--display-order")]
    public int? DisplayOrder { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}