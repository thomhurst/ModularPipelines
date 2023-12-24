using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("trustedadvisor", "list-organization-recommendation-resources")]
public record AwsTrustedadvisorListOrganizationRecommendationResourcesOptions(
[property: CommandSwitch("--organization-recommendation-identifier")] string OrganizationRecommendationIdentifier
) : AwsOptions
{
    [CommandSwitch("--affected-account-id")]
    public string? AffectedAccountId { get; set; }

    [CommandSwitch("--region-code")]
    public string? RegionCode { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}