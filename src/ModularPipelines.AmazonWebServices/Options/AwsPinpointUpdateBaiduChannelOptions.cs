using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pinpoint", "update-baidu-channel")]
public record AwsPinpointUpdateBaiduChannelOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--baidu-channel-request")] string BaiduChannelRequest
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}