using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iottwinmaker", "list-properties")]
public record AwsIottwinmakerListPropertiesOptions(
[property: CliOption("--workspace-id")] string WorkspaceId,
[property: CliOption("--entity-id")] string EntityId
) : AwsOptions
{
    [CliOption("--component-name")]
    public string? ComponentName { get; set; }

    [CliOption("--component-path")]
    public string? ComponentPath { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}