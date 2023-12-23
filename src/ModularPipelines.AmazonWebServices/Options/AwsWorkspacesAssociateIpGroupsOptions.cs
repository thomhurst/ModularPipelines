using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "associate-ip-groups")]
public record AwsWorkspacesAssociateIpGroupsOptions(
[property: CommandSwitch("--directory-id")] string DirectoryId,
[property: CommandSwitch("--group-ids")] string[] GroupIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}