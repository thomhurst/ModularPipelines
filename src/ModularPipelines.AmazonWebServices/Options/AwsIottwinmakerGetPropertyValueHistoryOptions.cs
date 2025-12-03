using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iottwinmaker", "get-property-value-history")]
public record AwsIottwinmakerGetPropertyValueHistoryOptions(
[property: CliOption("--workspace-id")] string WorkspaceId,
[property: CliOption("--selected-properties")] string[] SelectedProperties
) : AwsOptions
{
    [CliOption("--entity-id")]
    public string? EntityId { get; set; }

    [CliOption("--component-name")]
    public string? ComponentName { get; set; }

    [CliOption("--component-path")]
    public string? ComponentPath { get; set; }

    [CliOption("--component-type-id")]
    public string? ComponentTypeId { get; set; }

    [CliOption("--property-filters")]
    public string[]? PropertyFilters { get; set; }

    [CliOption("--start-date-time")]
    public long? StartDateTime { get; set; }

    [CliOption("--end-date-time")]
    public long? EndDateTime { get; set; }

    [CliOption("--interpolation")]
    public string? Interpolation { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--order-by-time")]
    public string? OrderByTime { get; set; }

    [CliOption("--start-time")]
    public string? StartTime { get; set; }

    [CliOption("--end-time")]
    public string? EndTime { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}