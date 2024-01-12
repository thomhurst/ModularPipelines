using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "tpus", "accelerator-types", "list")]
public record GcloudComputeTpusAcceleratorTypesListOptions : GcloudOptions
{
    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}