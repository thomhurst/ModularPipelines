using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bedrock-agent", "create-agent")]
public record AwsBedrockAgentCreateAgentOptions(
[property: CliOption("--agent-name")] string AgentName,
[property: CliOption("--agent-resource-role-arn")] string AgentResourceRoleArn
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

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

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--prompt-override-configuration")]
    public string? PromptOverrideConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}