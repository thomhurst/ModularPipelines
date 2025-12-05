using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "admin-link-provider-for-user")]
public record AwsCognitoIdpAdminLinkProviderForUserOptions(
[property: CliOption("--user-pool-id")] string UserPoolId,
[property: CliOption("--destination-user")] string DestinationUser,
[property: CliOption("--source-user")] string SourceUser
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}