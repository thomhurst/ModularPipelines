using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "get-endpoint")]
public record AwsPinpointGetEndpointOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--endpoint-id")] string EndpointId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}