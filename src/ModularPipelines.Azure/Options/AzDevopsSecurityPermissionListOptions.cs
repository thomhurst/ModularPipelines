using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops", "security", "permission", "list")]
public record AzDevopsSecurityPermissionListOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--subject")] string Subject
) : AzOptions
{
    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [BooleanCommandSwitch("--recurse")]
    public bool? Recurse { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }
}

