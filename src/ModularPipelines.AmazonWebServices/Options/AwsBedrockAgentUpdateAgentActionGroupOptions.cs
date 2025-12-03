using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bedrock-agent", "update-agent-action-group")]
public record AwsBedrockAgentUpdateAgentActionGroupOptions(
[property: CliOption("--agent-id")] string AgentId,
[property: CliOption("--agent-version")] string AgentVersion,
[property: CliOption("--action-group-id")] string ActionGroupId,
[property: CliOption("--action-group-name")] string ActionGroupName
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--parent-action-group-signature")]
    public string? ParentActionGroupSignature { get; set; }

    [CliOption("--action-group-executor")]
    public string? ActionGroupExecutor { get; set; }

    [CliOption("--action-group-state")]
    public string? ActionGroupState { get; set; }

    [CliOption("--api-schema")]
    public string? ApiSchema { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}