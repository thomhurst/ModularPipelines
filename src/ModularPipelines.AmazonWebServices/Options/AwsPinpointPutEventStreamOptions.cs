using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "put-event-stream")]
public record AwsPinpointPutEventStreamOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--write-event-stream")] string WriteEventStream
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}