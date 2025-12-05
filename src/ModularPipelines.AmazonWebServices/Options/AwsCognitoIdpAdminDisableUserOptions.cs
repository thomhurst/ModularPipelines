using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "admin-disable-user")]
public record AwsCognitoIdpAdminDisableUserOptions(
[property: CliOption("--user-pool-id")] string UserPoolId,
[property: CliOption("--username")] string Username
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}