using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devops", "security", "permission", "namespace", "list")]
public record AzDevopsSecurityPermissionNamespaceListOptions : AzOptions
{
    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [BooleanCommandSwitch("--local-only")]
    public bool? LocalOnly { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }
}