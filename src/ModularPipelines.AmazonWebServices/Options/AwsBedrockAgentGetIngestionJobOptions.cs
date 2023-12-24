using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bedrock-agent", "get-ingestion-job")]
public record AwsBedrockAgentGetIngestionJobOptions(
[property: CommandSwitch("--knowledge-base-id")] string KnowledgeBaseId,
[property: CommandSwitch("--data-source-id")] string DataSourceId,
[property: CommandSwitch("--ingestion-job-id")] string IngestionJobId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}