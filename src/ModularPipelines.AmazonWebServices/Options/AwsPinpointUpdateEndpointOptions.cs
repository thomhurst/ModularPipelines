using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "update-endpoint")]
public record AwsPinpointUpdateEndpointOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--endpoint-id")] string EndpointId,
[property: CliOption("--endpoint-request")] string EndpointRequest
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}