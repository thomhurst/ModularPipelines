using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognito-idp", "list-user-import-jobs")]
public record AwsCognitoIdpListUserImportJobsOptions(
[property: CliOption("--user-pool-id")] string UserPoolId,
[property: CliOption("--max-results")] int MaxResults
) : AwsOptions
{
    [CliOption("--pagination-token")]
    public string? PaginationToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}