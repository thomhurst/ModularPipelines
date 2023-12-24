using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("amp", "update-workspace-alias")]
public record AwsAmpUpdateWorkspaceAliasOptions(
[property: CommandSwitch("--workspace-id")] string WorkspaceId
) : AwsOptions
{
    [CommandSwitch("--alias")]
    public string? Alias { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}