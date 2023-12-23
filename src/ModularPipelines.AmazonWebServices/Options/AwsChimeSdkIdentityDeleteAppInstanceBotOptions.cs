using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-identity", "delete-app-instance-bot")]
public record AwsChimeSdkIdentityDeleteAppInstanceBotOptions(
[property: CommandSwitch("--app-instance-bot-arn")] string AppInstanceBotArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}