using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bedrock-agent", "update-agent-alias")]
public record AwsBedrockAgentUpdateAgentAliasOptions(
[property: CliOption("--agent-id")] string AgentId,
[property: CliOption("--agent-alias-id")] string AgentAliasId,
[property: CliOption("--agent-alias-name")] string AgentAliasName
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--routing-configuration")]
    public string[]? RoutingConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}