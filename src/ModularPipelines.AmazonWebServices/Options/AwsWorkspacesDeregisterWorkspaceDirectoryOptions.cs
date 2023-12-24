using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "deregister-workspace-directory")]
public record AwsWorkspacesDeregisterWorkspaceDirectoryOptions(
[property: CommandSwitch("--directory-id")] string DirectoryId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}