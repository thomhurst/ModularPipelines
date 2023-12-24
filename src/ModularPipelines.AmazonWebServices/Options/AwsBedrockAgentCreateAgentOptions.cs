using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bedrock-agent", "create-agent")]
public record AwsBedrockAgentCreateAgentOptions(
[property: CommandSwitch("--agent-name")] string AgentName,
[property: CommandSwitch("--agent-resource-role-arn")] string AgentResourceRoleArn
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

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

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--prompt-override-configuration")]
    public string? PromptOverrideConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}