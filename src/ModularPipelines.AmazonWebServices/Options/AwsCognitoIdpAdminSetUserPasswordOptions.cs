using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "admin-set-user-password")]
public record AwsCognitoIdpAdminSetUserPasswordOptions(
[property: CliOption("--user-pool-id")] string UserPoolId,
[property: CliOption("--username")] string Username,
[property: CliOption("--password")] string Password
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}