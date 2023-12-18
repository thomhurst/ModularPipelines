using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aosm", "nsd", "generate-config")]
public record AzAosmNsdGenerateConfigOptions : AzOptions
{
    [CommandSwitch("--output-file")]
    public string? OutputFile { get; set; }
}