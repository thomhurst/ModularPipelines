using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iottwinmaker", "get-property-value")]
public record AwsIottwinmakerGetPropertyValueOptions(
[property: CliOption("--selected-properties")] string[] SelectedProperties,
[property: CliOption("--workspace-id")] string WorkspaceId
) : AwsOptions
{
    [CliOption("--component-name")]
    public string? ComponentName { get; set; }

    [CliOption("--component-path")]
    public string? ComponentPath { get; set; }

    [CliOption("--component-type-id")]
    public string? ComponentTypeId { get; set; }

    [CliOption("--entity-id")]
    public string? EntityId { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--property-group-name")]
    public string? PropertyGroupName { get; set; }

    [CliOption("--tabular-conditions")]
    public string? TabularConditions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}