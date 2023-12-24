using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bedrock-agent", "update-agent-alias")]
public record AwsBedrockAgentUpdateAgentAliasOptions(
[property: CommandSwitch("--agent-id")] string AgentId,
[property: CommandSwitch("--agent-alias-id")] string AgentAliasId,
[property: CommandSwitch("--agent-alias-name")] string AgentAliasName
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--routing-configuration")]
    public string[]? RoutingConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}