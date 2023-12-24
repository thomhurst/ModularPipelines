using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "create-ip-group")]
public record AwsWorkspacesCreateIpGroupOptions(
[property: CommandSwitch("--group-name")] string GroupName
) : AwsOptions
{
    [CommandSwitch("--group-desc")]
    public string? GroupDesc { get; set; }

    [CommandSwitch("--user-rules")]
    public string[]? UserRules { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}