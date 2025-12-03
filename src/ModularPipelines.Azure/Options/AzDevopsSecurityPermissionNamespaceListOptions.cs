using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devops", "security", "permission", "namespace", "list")]
public record AzDevopsSecurityPermissionNamespaceListOptions : AzOptions
{
    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliFlag("--local-only")]
    public bool? LocalOnly { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }
}