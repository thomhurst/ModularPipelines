using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-identity", "delete-app-instance-bot")]
public record AwsChimeSdkIdentityDeleteAppInstanceBotOptions(
[property: CliOption("--app-instance-bot-arn")] string AppInstanceBotArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}