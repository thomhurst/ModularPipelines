using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devops-guru", "describe-organization-resource-collection-health")]
public record AwsDevopsGuruDescribeOrganizationResourceCollectionHealthOptions(
[property: CliOption("--organization-resource-collection-type")] string OrganizationResourceCollectionType
) : AwsOptions
{
    [CliOption("--account-ids")]
    public string[]? AccountIds { get; set; }

    [CliOption("--organizational-unit-ids")]
    public string[]? OrganizationalUnitIds { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}