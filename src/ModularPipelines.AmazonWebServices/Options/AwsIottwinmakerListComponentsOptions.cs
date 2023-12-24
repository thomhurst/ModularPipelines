using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iottwinmaker", "list-components")]
public record AwsIottwinmakerListComponentsOptions(
[property: CommandSwitch("--workspace-id")] string WorkspaceId,
[property: CommandSwitch("--entity-id")] string EntityId
) : AwsOptions
{
    [CommandSwitch("--component-path")]
    public string? ComponentPath { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}