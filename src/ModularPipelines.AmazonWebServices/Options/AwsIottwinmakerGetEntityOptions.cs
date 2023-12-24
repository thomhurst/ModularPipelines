using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iottwinmaker", "get-entity")]
public record AwsIottwinmakerGetEntityOptions(
[property: CommandSwitch("--workspace-id")] string WorkspaceId,
[property: CommandSwitch("--entity-id")] string EntityId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}