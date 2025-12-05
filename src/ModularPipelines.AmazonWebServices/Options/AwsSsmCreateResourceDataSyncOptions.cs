using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "create-resource-data-sync")]
public record AwsSsmCreateResourceDataSyncOptions(
[property: CliOption("--sync-name")] string SyncName
) : AwsOptions
{
    [CliOption("--s3-destination")]
    public string? S3Destination { get; set; }

    [CliOption("--sync-type")]
    public string? SyncType { get; set; }

    [CliOption("--sync-source")]
    public string? SyncSource { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}