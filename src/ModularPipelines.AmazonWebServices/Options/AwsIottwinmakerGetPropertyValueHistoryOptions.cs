using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iottwinmaker", "get-property-value-history")]
public record AwsIottwinmakerGetPropertyValueHistoryOptions(
[property: CommandSwitch("--workspace-id")] string WorkspaceId,
[property: CommandSwitch("--selected-properties")] string[] SelectedProperties
) : AwsOptions
{
    [CommandSwitch("--entity-id")]
    public string? EntityId { get; set; }

    [CommandSwitch("--component-name")]
    public string? ComponentName { get; set; }

    [CommandSwitch("--component-path")]
    public string? ComponentPath { get; set; }

    [CommandSwitch("--component-type-id")]
    public string? ComponentTypeId { get; set; }

    [CommandSwitch("--property-filters")]
    public string[]? PropertyFilters { get; set; }

    [CommandSwitch("--start-date-time")]
    public long? StartDateTime { get; set; }

    [CommandSwitch("--end-date-time")]
    public long? EndDateTime { get; set; }

    [CommandSwitch("--interpolation")]
    public string? Interpolation { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--order-by-time")]
    public string? OrderByTime { get; set; }

    [CommandSwitch("--start-time")]
    public string? StartTime { get; set; }

    [CommandSwitch("--end-time")]
    public string? EndTime { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}