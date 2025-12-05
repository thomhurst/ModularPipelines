using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bedrock-agent", "update-agent")]
public record AwsBedrockAgentUpdateAgentOptions(
[property: CliOption("--agent-id")] string AgentId,
[property: CliOption("--agent-name")] string AgentName,
[property: CliOption("--agent-resource-role-arn")] string AgentResourceRoleArn
) : AwsOptions
{
    [CliOption("--instruction")]
    public string? Instruction { get; set; }

    [CliOption("--foundation-model")]
    public string? FoundationModel { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--idle-session-ttl-in-seconds")]
    public int? IdleSessionTtlInSeconds { get; set; }

    [CliOption("--customer-encryption-key-arn")]
    public string? CustomerEncryptionKeyArn { get; set; }

    [CliOption("--prompt-override-configuration")]
    public string? PromptOverrideConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}