using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "admin-list-devices")]
public record AwsCognitoIdpAdminListDevicesOptions(
[property: CliOption("--user-pool-id")] string UserPoolId,
[property: CliOption("--username")] string Username
) : AwsOptions
{
    [CliOption("--limit")]
    public int? Limit { get; set; }

    [CliOption("--pagination-token")]
    public string? PaginationToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}