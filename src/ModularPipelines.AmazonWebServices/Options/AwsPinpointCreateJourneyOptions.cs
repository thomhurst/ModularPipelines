using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "create-journey")]
public record AwsPinpointCreateJourneyOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--write-journey-request")] string WriteJourneyRequest
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}