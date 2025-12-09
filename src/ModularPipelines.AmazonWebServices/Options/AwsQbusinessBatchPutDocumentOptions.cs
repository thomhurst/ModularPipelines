using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qbusiness", "batch-put-document")]
public record AwsQbusinessBatchPutDocumentOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--documents")] string[] Documents,
[property: CliOption("--index-id")] string IndexId
) : AwsOptions
{
    [CliOption("--data-source-sync-id")]
    public string? DataSourceSyncId { get; set; }

    [CliOption("--role-arn")]
    public string? RoleArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}