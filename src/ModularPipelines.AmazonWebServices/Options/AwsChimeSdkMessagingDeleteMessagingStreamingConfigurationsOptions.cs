using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-messaging", "delete-messaging-streaming-configurations")]
public record AwsChimeSdkMessagingDeleteMessagingStreamingConfigurationsOptions(
[property: CliOption("--app-instance-arn")] string AppInstanceArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}