using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "update-endpoints-batch")]
public record AwsPinpointUpdateEndpointsBatchOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--endpoint-batch-request")] string EndpointBatchRequest
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}