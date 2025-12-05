using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "delete-endpoint")]
public record AwsPinpointDeleteEndpointOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--endpoint-id")] string EndpointId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}