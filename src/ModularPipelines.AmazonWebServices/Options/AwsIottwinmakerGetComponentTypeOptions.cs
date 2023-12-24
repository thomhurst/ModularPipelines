using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iottwinmaker", "get-component-type")]
public record AwsIottwinmakerGetComponentTypeOptions(
[property: CommandSwitch("--workspace-id")] string WorkspaceId,
[property: CommandSwitch("--component-type-id")] string ComponentTypeId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}