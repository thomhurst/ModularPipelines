using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops", "security", "permission", "namespace", "list")]
public record AzDevopsSecurityPermissionNamespaceListOptions(
[property: CommandSwitch("--id")] string Id
) : AzOptions
{
    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [BooleanCommandSwitch("--local-only")]
    public bool? LocalOnly { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }
}