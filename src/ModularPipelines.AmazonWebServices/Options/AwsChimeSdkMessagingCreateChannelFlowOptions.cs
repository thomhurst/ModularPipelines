using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-messaging", "create-channel-flow")]
public record AwsChimeSdkMessagingCreateChannelFlowOptions(
[property: CliOption("--app-instance-arn")] string AppInstanceArn,
[property: CliOption("--processors")] string[] Processors,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}