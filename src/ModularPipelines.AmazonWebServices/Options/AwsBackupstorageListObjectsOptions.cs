using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backupstorage", "list-objects")]
public record AwsBackupstorageListObjectsOptions(
[property: CliOption("--storage-job-id")] string StorageJobId
) : AwsOptions
{
    [CliOption("--starting-object-name")]
    public string? StartingObjectName { get; set; }

    [CliOption("--starting-object-prefix")]
    public string? StartingObjectPrefix { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--created-before")]
    public long? CreatedBefore { get; set; }

    [CliOption("--created-after")]
    public long? CreatedAfter { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}