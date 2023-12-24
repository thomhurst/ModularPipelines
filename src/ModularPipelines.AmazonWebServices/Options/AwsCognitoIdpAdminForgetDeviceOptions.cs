using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-idp", "admin-forget-device")]
public record AwsCognitoIdpAdminForgetDeviceOptions(
[property: CommandSwitch("--user-pool-id")] string UserPoolId,
[property: CommandSwitch("--username")] string Username,
[property: CommandSwitch("--device-key")] string DeviceKey
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}