using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bedrock-agent", "create-agent-alias")]
public record AwsBedrockAgentCreateAgentAliasOptions(
[property: CliOption("--agent-id")] string AgentId,
[property: CliOption("--agent-alias-name")] string AgentAliasName
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--routing-configuration")]
    public string[]? RoutingConfiguration { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}