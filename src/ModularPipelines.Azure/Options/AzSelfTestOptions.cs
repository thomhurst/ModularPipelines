using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("self-test")]
public record AzSelfTestOptions : AzOptions
{
    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [CommandSwitch("--yes")]
    public bool? Yes { get; set; } = true;
}