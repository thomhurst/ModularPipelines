using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "admin-forget-device")]
public record AwsCognitoIdpAdminForgetDeviceOptions(
[property: CliOption("--user-pool-id")] string UserPoolId,
[property: CliOption("--username")] string Username,
[property: CliOption("--device-key")] string DeviceKey
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}