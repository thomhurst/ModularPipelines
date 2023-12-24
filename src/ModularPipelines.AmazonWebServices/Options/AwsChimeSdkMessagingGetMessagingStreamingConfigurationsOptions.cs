using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-messaging", "get-messaging-streaming-configurations")]
public record AwsChimeSdkMessagingGetMessagingStreamingConfigurationsOptions(
[property: CommandSwitch("--app-instance-arn")] string AppInstanceArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}