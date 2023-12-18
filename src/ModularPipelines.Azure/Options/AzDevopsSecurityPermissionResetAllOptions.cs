using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops", "security", "permission", "reset-all")]
public record AzDevopsSecurityPermissionResetAllOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--subject")] string Subject,
[property: CommandSwitch("--token")] string Token
) : AzOptions
{
    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}