using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rum", "put-rum-events")]
public record AwsRumPutRumEventsOptions(
[property: CliOption("--app-monitor-details")] string AppMonitorDetails,
[property: CliOption("--batch-id")] string BatchId,
[property: CliOption("--id")] string Id,
[property: CliOption("--rum-events")] string[] RumEvents,
[property: CliOption("--user-details")] string UserDetails
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}