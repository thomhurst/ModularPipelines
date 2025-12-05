using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bedrock-agent", "update-data-source")]
public record AwsBedrockAgentUpdateDataSourceOptions(
[property: CliOption("--knowledge-base-id")] string KnowledgeBaseId,
[property: CliOption("--data-source-id")] string DataSourceId,
[property: CliOption("--name")] string Name,
[property: CliOption("--data-source-configuration")] string DataSourceConfiguration
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--server-side-encryption-configuration")]
    public string? ServerSideEncryptionConfiguration { get; set; }

    [CliOption("--vector-ingestion-configuration")]
    public string? VectorIngestionConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}