using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bicep", "decompile")]
public record AzBicepDecompileOptions(
[property: CommandSwitch("--file")] string File
) : AzOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }
}