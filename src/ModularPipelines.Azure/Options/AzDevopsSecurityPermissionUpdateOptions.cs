using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops", "security", "permission", "update")]
public record AzDevopsSecurityPermissionUpdateOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--subject")] string Subject,
[property: CommandSwitch("--token")] string Token
) : AzOptions
{
    [BooleanCommandSwitch("--allow-bit")]
    public bool? AllowBit { get; set; }

    [CommandSwitch("--deny-bit")]
    public string? DenyBit { get; set; }

    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [BooleanCommandSwitch("--merge")]
    public bool? Merge { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }
}

