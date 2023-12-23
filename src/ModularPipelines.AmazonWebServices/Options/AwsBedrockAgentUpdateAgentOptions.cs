using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bedrock-agent", "update-agent")]
public record AwsBedrockAgentUpdateAgentOptions(
[property: CommandSwitch("--agent-id")] string AgentId,
[property: CommandSwitch("--agent-name")] string AgentName,
[property: CommandSwitch("--agent-resource-role-arn")] string AgentResourceRoleArn
) : AwsOptions
{
    [CommandSwitch("--instruction")]
    public string? Instruction { get; set; }

    [CommandSwitch("--foundation-model")]
    public string? FoundationModel { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--idle-session-ttl-in-seconds")]
    public int? IdleSessionTtlInSeconds { get; set; }

    [CommandSwitch("--customer-encryption-key-arn")]
    public string? CustomerEncryptionKeyArn { get; set; }

    [CommandSwitch("--prompt-override-configuration")]
    public string? PromptOverrideConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}