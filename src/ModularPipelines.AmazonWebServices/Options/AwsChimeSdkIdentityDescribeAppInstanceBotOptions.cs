using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime-sdk-identity", "describe-app-instance-bot")]
public record AwsChimeSdkIdentityDescribeAppInstanceBotOptions(
[property: CliOption("--app-instance-bot-arn")] string AppInstanceBotArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}