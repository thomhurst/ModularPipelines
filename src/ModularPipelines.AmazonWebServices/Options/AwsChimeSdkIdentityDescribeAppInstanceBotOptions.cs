using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-identity", "describe-app-instance-bot")]
public record AwsChimeSdkIdentityDescribeAppInstanceBotOptions(
[property: CommandSwitch("--app-instance-bot-arn")] string AppInstanceBotArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}