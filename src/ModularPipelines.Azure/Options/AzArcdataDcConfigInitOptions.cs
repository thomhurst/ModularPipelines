using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arcdata", "dc", "config", "init")]
public record AzArcdataDcConfigInitOptions : AzOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }
}