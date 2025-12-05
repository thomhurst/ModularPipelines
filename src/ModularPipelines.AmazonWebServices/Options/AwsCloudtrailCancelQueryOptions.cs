using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudtrail", "cancel-query")]
public record AwsCloudtrailCancelQueryOptions(
[property: CliOption("--query-id")] string QueryId
) : AwsOptions
{
    [CliOption("--event-data-store")]
    public string? EventDataStore { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}