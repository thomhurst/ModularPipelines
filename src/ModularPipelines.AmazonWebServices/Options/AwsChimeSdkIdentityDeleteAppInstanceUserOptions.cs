using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-identity", "delete-app-instance-user")]
public record AwsChimeSdkIdentityDeleteAppInstanceUserOptions(
[property: CommandSwitch("--app-instance-user-arn")] string AppInstanceUserArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}