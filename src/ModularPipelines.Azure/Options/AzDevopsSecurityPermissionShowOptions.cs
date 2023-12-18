using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops", "security", "permission", "show")]
public record AzDevopsSecurityPermissionShowOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--subject")] string Subject,
[property: CommandSwitch("--token")] string Token
) : AzOptions
{
    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }
}