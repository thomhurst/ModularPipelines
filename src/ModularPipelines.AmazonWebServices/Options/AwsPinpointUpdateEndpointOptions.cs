using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "update-endpoint")]
public record AwsPinpointUpdateEndpointOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--endpoint-id")] string EndpointId,
[property: CommandSwitch("--endpoint-request")] string EndpointRequest
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}