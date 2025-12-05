using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qbusiness", "batch-delete-document")]
public record AwsQbusinessBatchDeleteDocumentOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--documents")] string[] Documents,
[property: CliOption("--index-id")] string IndexId
) : AwsOptions
{
    [CliOption("--data-source-sync-id")]
    public string? DataSourceSyncId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}