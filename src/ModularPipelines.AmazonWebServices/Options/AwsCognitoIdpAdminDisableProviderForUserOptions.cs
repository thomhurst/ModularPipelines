using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "admin-disable-provider-for-user")]
public record AwsCognitoIdpAdminDisableProviderForUserOptions(
[property: CliOption("--user-pool-id")] string UserPoolId,
[property: CliOption("--user")] string User
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}