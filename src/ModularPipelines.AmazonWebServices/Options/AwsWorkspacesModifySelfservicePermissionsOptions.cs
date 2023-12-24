using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "modify-selfservice-permissions")]
public record AwsWorkspacesModifySelfservicePermissionsOptions(
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--selfservice-permissions")] string SelfservicePermissions
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}