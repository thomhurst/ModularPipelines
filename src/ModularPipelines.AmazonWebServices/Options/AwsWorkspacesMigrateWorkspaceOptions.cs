using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "migrate-workspace")]
public record AwsWorkspacesMigrateWorkspaceOptions(
[property: CommandSwitch("--source-workspace-id")] string SourceWorkspaceId,
[property: CommandSwitch("--bundle-id")] string BundleId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}