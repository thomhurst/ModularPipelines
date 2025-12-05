using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudtrail", "start-event-data-store-ingestion")]
public record AwsCloudtrailStartEventDataStoreIngestionOptions(
[property: CliOption("--event-data-store")] string EventDataStore
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}