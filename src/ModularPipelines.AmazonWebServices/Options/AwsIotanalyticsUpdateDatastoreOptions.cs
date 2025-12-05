using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotanalytics", "update-datastore")]
public record AwsIotanalyticsUpdateDatastoreOptions(
[property: CliOption("--datastore-name")] string DatastoreName
) : AwsOptions
{
    [CliOption("--retention-period")]
    public string? RetentionPeriod { get; set; }

    [CliOption("--datastore-storage")]
    public string? DatastoreStorage { get; set; }

    [CliOption("--file-format-configuration")]
    public string? FileFormatConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}