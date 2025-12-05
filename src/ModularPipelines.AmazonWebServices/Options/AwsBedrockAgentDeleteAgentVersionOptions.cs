using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bedrock-agent", "delete-agent-version")]
public record AwsBedrockAgentDeleteAgentVersionOptions(
[property: CliOption("--agent-id")] string AgentId,
[property: CliOption("--agent-version")] string AgentVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}