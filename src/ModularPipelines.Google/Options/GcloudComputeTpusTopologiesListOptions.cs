using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "tpus", "topologies", "list")]
public record GcloudComputeTpusTopologiesListOptions : GcloudOptions
{
    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}