using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-messaging", "put-messaging-streaming-configurations")]
public record AwsChimeSdkMessagingPutMessagingStreamingConfigurationsOptions(
[property: CliOption("--app-instance-arn")] string AppInstanceArn,
[property: CliOption("--streaming-configurations")] string[] StreamingConfigurations
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}