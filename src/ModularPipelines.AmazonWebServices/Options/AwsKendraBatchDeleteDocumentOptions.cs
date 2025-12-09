using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kendra", "batch-delete-document")]
public record AwsKendraBatchDeleteDocumentOptions(
[property: CliOption("--index-id")] string IndexId,
[property: CliOption("--document-id-list")] string[] DocumentIdList
) : AwsOptions
{
    [CliOption("--data-source-sync-job-metric-target")]
    public string? DataSourceSyncJobMetricTarget { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}