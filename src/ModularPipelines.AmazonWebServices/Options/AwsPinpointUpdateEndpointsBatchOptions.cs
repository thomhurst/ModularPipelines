using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "update-endpoints-batch")]
public record AwsPinpointUpdateEndpointsBatchOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--endpoint-batch-request")] string EndpointBatchRequest
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}