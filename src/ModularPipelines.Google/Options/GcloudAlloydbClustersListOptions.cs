using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("alloydb", "clusters", "list")]
public record GcloudAlloydbClustersListOptions : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}