using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotanalytics", "create-datastore")]
public record AwsIotanalyticsCreateDatastoreOptions(
[property: CliOption("--datastore-name")] string DatastoreName
) : AwsOptions
{
    [CliOption("--datastore-storage")]
    public string? DatastoreStorage { get; set; }

    [CliOption("--retention-period")]
    public string? RetentionPeriod { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--file-format-configuration")]
    public string? FileFormatConfiguration { get; set; }

    [CliOption("--datastore-partitions")]
    public string? DatastorePartitions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}