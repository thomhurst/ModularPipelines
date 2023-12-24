using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudtrail", "start-event-data-store-ingestion")]
public record AwsCloudtrailStartEventDataStoreIngestionOptions(
[property: CommandSwitch("--event-data-store")] string EventDataStore
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}