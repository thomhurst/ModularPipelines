using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sso", "get-role-credentials")]
public record AwsSsoGetRoleCredentialsOptions(
[property: CliOption("--role-name")] string RoleName,
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--access-token")] string AccessToken
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}