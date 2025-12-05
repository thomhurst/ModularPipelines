using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("health", "describe-event-details-for-organization")]
public record AwsHealthDescribeEventDetailsForOrganizationOptions(
[property: CliOption("--organization-event-detail-filters")] string[] OrganizationEventDetailFilters
) : AwsOptions
{
    [CliOption("--locale")]
    public string? Locale { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}