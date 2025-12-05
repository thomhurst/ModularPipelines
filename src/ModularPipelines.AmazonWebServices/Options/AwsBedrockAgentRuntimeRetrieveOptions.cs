using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bedrock-agent-runtime", "retrieve")]
public record AwsBedrockAgentRuntimeRetrieveOptions(
[property: CliOption("--knowledge-base-id")] string KnowledgeBaseId,
[property: CliOption("--retrieval-query")] string RetrievalQuery
) : AwsOptions
{
    [CliOption("--retrieval-configuration")]
    public string? RetrievalConfiguration { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}