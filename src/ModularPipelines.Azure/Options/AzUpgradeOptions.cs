using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("upgrade")]
public record AzUpgradeOptions : AzOptions
{
    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [CommandSwitch("--yes")]
    public bool? Yes { get; set; } = true;
}