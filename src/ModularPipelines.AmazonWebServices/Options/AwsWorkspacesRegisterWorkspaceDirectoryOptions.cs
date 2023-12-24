using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "register-workspace-directory")]
public record AwsWorkspacesRegisterWorkspaceDirectoryOptions(
[property: CommandSwitch("--directory-id")] string DirectoryId
) : AwsOptions
{
    [CommandSwitch("--subnet-ids")]
    public string[]? SubnetIds { get; set; }

    [CommandSwitch("--tenancy")]
    public string? Tenancy { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}