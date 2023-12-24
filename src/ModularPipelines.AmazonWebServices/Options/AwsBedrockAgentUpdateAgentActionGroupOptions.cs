using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bedrock-agent", "update-agent-action-group")]
public record AwsBedrockAgentUpdateAgentActionGroupOptions(
[property: CommandSwitch("--agent-id")] string AgentId,
[property: CommandSwitch("--agent-version")] string AgentVersion,
[property: CommandSwitch("--action-group-id")] string ActionGroupId,
[property: CommandSwitch("--action-group-name")] string ActionGroupName
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--parent-action-group-signature")]
    public string? ParentActionGroupSignature { get; set; }

    [CommandSwitch("--action-group-executor")]
    public string? ActionGroupExecutor { get; set; }

    [CommandSwitch("--action-group-state")]
    public string? ActionGroupState { get; set; }

    [CommandSwitch("--api-schema")]
    public string? ApiSchema { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}