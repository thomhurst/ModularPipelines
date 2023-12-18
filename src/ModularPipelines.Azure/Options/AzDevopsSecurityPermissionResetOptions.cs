using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops", "security", "permission", "reset")]
public record AzDevopsSecurityPermissionResetOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--permission-bit")] string PermissionBit,
[property: CommandSwitch("--subject")] string Subject,
[property: CommandSwitch("--token")] string Token
) : AzOptions
{
    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }
}