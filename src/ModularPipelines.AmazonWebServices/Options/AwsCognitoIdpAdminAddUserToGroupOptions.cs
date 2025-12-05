using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "admin-add-user-to-group")]
public record AwsCognitoIdpAdminAddUserToGroupOptions(
[property: CliOption("--user-pool-id")] string UserPoolId,
[property: CliOption("--username")] string Username,
[property: CliOption("--group-name")] string GroupName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}