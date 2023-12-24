using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotanalytics", "create-datastore")]
public record AwsIotanalyticsCreateDatastoreOptions(
[property: CommandSwitch("--datastore-name")] string DatastoreName
) : AwsOptions
{
    [CommandSwitch("--datastore-storage")]
    public string? DatastoreStorage { get; set; }

    [CommandSwitch("--retention-period")]
    public string? RetentionPeriod { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--file-format-configuration")]
    public string? FileFormatConfiguration { get; set; }

    [CommandSwitch("--datastore-partitions")]
    public string? DatastorePartitions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}