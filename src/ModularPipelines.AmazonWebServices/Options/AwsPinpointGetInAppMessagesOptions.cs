using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "get-in-app-messages")]
public record AwsPinpointGetInAppMessagesOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--endpoint-id")] string EndpointId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}