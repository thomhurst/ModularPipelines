using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotanalytics", "delete-datastore")]
public record AwsIotanalyticsDeleteDatastoreOptions(
[property: CommandSwitch("--datastore-name")] string DatastoreName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}