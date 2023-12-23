using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pinpoint", "update-baidu-channel")]
public record AwsPinpointUpdateBaiduChannelOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--baidu-channel-request")] string BaiduChannelRequest
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}