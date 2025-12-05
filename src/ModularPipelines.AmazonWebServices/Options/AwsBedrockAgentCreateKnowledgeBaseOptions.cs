using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bedrock-agent", "create-knowledge-base")]
public record AwsBedrockAgentCreateKnowledgeBaseOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--role-arn")] string RoleArn,
[property: CliOption("--knowledge-base-configuration")] string KnowledgeBaseConfiguration,
[property: CliOption("--storage-configuration")] string StorageConfiguration
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}