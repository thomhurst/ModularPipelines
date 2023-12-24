using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-identity", "update-app-instance-bot")]
public record AwsChimeSdkIdentityUpdateAppInstanceBotOptions(
[property: CommandSwitch("--app-instance-bot-arn")] string AppInstanceBotArn,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--metadata")] string Metadata
) : AwsOptions
{
    [CommandSwitch("--configuration")]
    public string? Configuration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}