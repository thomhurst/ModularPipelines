using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-messaging", "put-messaging-streaming-configurations")]
public record AwsChimeSdkMessagingPutMessagingStreamingConfigurationsOptions(
[property: CommandSwitch("--app-instance-arn")] string AppInstanceArn,
[property: CommandSwitch("--streaming-configurations")] string[] StreamingConfigurations
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}