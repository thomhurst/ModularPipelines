using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bedrock-agent", "get-agent-action-group")]
public record AwsBedrockAgentGetAgentActionGroupOptions(
[property: CliOption("--agent-id")] string AgentId,
[property: CliOption("--agent-version")] string AgentVersion,
[property: CliOption("--action-group-id")] string ActionGroupId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}