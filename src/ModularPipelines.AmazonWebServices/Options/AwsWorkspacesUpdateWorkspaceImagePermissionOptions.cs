using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "update-workspace-image-permission")]
public record AwsWorkspacesUpdateWorkspaceImagePermissionOptions(
[property: CommandSwitch("--image-id")] string ImageId,
[property: CommandSwitch("--shared-account-id")] string SharedAccountId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}