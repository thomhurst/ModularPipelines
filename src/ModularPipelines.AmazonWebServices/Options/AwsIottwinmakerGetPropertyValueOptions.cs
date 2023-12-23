using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iottwinmaker", "get-property-value")]
public record AwsIottwinmakerGetPropertyValueOptions(
[property: CommandSwitch("--selected-properties")] string[] SelectedProperties,
[property: CommandSwitch("--workspace-id")] string WorkspaceId
) : AwsOptions
{
    [CommandSwitch("--component-name")]
    public string? ComponentName { get; set; }

    [CommandSwitch("--component-path")]
    public string? ComponentPath { get; set; }

    [CommandSwitch("--component-type-id")]
    public string? ComponentTypeId { get; set; }

    [CommandSwitch("--entity-id")]
    public string? EntityId { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--property-group-name")]
    public string? PropertyGroupName { get; set; }

    [CommandSwitch("--tabular-conditions")]
    public string? TabularConditions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}