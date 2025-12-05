using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudtrail", "delete-event-data-store")]
public record AwsCloudtrailDeleteEventDataStoreOptions(
[property: CliOption("--event-data-store")] string EventDataStore
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}