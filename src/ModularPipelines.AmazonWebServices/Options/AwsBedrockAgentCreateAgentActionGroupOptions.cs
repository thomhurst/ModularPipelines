using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bedrock-agent", "create-agent-action-group")]
public record AwsBedrockAgentCreateAgentActionGroupOptions(
[property: CliOption("--agent-id")] string AgentId,
[property: CliOption("--agent-version")] string AgentVersion,
[property: CliOption("--action-group-name")] string ActionGroupName
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--parent-action-group-signature")]
    public string? ParentActionGroupSignature { get; set; }

    [CliOption("--action-group-executor")]
    public string? ActionGroupExecutor { get; set; }

    [CliOption("--api-schema")]
    public string? ApiSchema { get; set; }

    [CliOption("--action-group-state")]
    public string? ActionGroupState { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}