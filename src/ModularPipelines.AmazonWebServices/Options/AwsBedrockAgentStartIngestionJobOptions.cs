using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bedrock-agent", "start-ingestion-job")]
public record AwsBedrockAgentStartIngestionJobOptions(
[property: CliOption("--knowledge-base-id")] string KnowledgeBaseId,
[property: CliOption("--data-source-id")] string DataSourceId
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}