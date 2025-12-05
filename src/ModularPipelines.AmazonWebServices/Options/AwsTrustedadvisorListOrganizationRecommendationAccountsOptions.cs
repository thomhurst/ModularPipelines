using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("trustedadvisor", "list-organization-recommendation-accounts")]
public record AwsTrustedadvisorListOrganizationRecommendationAccountsOptions(
[property: CliOption("--organization-recommendation-identifier")] string OrganizationRecommendationIdentifier
) : AwsOptions
{
    [CliOption("--affected-account-id")]
    public string? AffectedAccountId { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}