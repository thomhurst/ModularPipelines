using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognito-sync", "register-device")]
public record AwsCognitoSyncRegisterDeviceOptions(
[property: CommandSwitch("--identity-pool-id")] string IdentityPoolId,
[property: CommandSwitch("--identity-id")] string IdentityId,
[property: CommandSwitch("--platform")] string Platform,
[property: CommandSwitch("--token")] string Token
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}