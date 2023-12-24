using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotanalytics", "update-datastore")]
public record AwsIotanalyticsUpdateDatastoreOptions(
[property: CommandSwitch("--datastore-name")] string DatastoreName
) : AwsOptions
{
    [CommandSwitch("--retention-period")]
    public string? RetentionPeriod { get; set; }

    [CommandSwitch("--datastore-storage")]
    public string? DatastoreStorage { get; set; }

    [CommandSwitch("--file-format-configuration")]
    public string? FileFormatConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}