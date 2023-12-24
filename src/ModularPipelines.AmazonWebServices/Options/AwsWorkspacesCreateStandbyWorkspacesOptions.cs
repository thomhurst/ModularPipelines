using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "create-standby-workspaces")]
public record AwsWorkspacesCreateStandbyWorkspacesOptions(
[property: CommandSwitch("--primary-region")] string PrimaryRegion,
[property: CommandSwitch("--standby-workspaces")] string[] StandbyWorkspaces
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}