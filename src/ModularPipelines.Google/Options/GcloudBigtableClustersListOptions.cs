using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bigtable", "clusters", "list")]
public record GcloudBigtableClustersListOptions : GcloudOptions
{
    [CommandSwitch("--instances")]
    public string[]? Instances { get; set; }
}