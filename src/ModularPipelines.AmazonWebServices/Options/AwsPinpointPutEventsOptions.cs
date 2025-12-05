using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "put-events")]
public record AwsPinpointPutEventsOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--events-request")] string EventsRequest
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}