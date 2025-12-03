using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("s3control", "list-access-grants")]
public record AwsS3controlListAccessGrantsOptions(
[property: CliOption("--account-id")] string AccountId
) : AwsOptions
{
    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--grantee-type")]
    public string? GranteeType { get; set; }

    [CliOption("--grantee-identifier")]
    public string? GranteeIdentifier { get; set; }

    [CliOption("--permission")]
    public string? Permission { get; set; }

    [CliOption("--grant-scope")]
    public string? GrantScope { get; set; }

    [CliOption("--application-arn")]
    public string? ApplicationArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}