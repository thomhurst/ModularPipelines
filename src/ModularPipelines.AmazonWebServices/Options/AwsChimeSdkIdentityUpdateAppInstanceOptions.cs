using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-identity", "update-app-instance")]
public record AwsChimeSdkIdentityUpdateAppInstanceOptions(
[property: CommandSwitch("--app-instance-arn")] string AppInstanceArn,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--metadata")] string Metadata
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}