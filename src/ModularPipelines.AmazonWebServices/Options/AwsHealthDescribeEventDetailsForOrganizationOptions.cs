using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("health", "describe-event-details-for-organization")]
public record AwsHealthDescribeEventDetailsForOrganizationOptions(
[property: CommandSwitch("--organization-event-detail-filters")] string[] OrganizationEventDetailFilters
) : AwsOptions
{
    [CommandSwitch("--locale")]
    public string? Locale { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}