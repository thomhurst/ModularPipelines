using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("wisdom", "get-import-job")]
public record AwsWisdomGetImportJobOptions(
[property: CliOption("--import-job-id")] string ImportJobId,
[property: CliOption("--knowledge-base-id")] string KnowledgeBaseId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}