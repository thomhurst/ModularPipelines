using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmail", "list-groups-for-entity")]
public record AwsWorkmailListGroupsForEntityOptions(
[property: CliOption("--organization-id")] string OrganizationId,
[property: CliOption("--entity-id")] string EntityId
) : AwsOptions
{
    [CliOption("--filters")]
    public string? Filters { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}