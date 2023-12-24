using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bedrock-agent", "create-agent-action-group")]
public record AwsBedrockAgentCreateAgentActionGroupOptions(
[property: CommandSwitch("--agent-id")] string AgentId,
[property: CommandSwitch("--agent-version")] string AgentVersion,
[property: CommandSwitch("--action-group-name")] string ActionGroupName
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--parent-action-group-signature")]
    public string? ParentActionGroupSignature { get; set; }

    [CommandSwitch("--action-group-executor")]
    public string? ActionGroupExecutor { get; set; }

    [CommandSwitch("--api-schema")]
    public string? ApiSchema { get; set; }

    [CommandSwitch("--action-group-state")]
    public string? ActionGroupState { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}