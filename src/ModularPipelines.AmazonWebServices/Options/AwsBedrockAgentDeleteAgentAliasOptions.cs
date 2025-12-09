using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bedrock-agent", "delete-agent-alias")]
public record AwsBedrockAgentDeleteAgentAliasOptions(
[property: CliOption("--agent-id")] string AgentId,
[property: CliOption("--agent-alias-id")] string AgentAliasId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}