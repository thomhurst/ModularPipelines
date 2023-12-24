using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workspaces", "revoke-ip-rules")]
public record AwsWorkspacesRevokeIpRulesOptions(
[property: CommandSwitch("--group-id")] string GroupId,
[property: CommandSwitch("--user-rules")] string[] UserRules
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}