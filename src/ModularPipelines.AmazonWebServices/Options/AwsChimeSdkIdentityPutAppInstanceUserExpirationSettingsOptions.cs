using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("chime-sdk-identity", "put-app-instance-user-expiration-settings")]
public record AwsChimeSdkIdentityPutAppInstanceUserExpirationSettingsOptions(
[property: CommandSwitch("--app-instance-user-arn")] string AppInstanceUserArn
) : AwsOptions
{
    [CommandSwitch("--expiration-settings")]
    public string? ExpirationSettings { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}