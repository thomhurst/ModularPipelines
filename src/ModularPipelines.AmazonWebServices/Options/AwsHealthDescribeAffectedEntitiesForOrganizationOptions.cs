using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("health", "describe-affected-entities-for-organization")]
public record AwsHealthDescribeAffectedEntitiesForOrganizationOptions : AwsOptions
{
    [CliOption("--organization-entity-filters")]
    public string[]? OrganizationEntityFilters { get; set; }

    [CliOption("--locale")]
    public string? Locale { get; set; }

    [CliOption("--organization-entity-account-filters")]
    public string[]? OrganizationEntityAccountFilters { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}