using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("organizations", "describe-create-account-status")]
public record AwsOrganizationsDescribeCreateAccountStatusOptions(
[property: CliOption("--create-account-request-id")] string CreateAccountRequestId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}