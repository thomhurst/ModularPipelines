using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("next")]
public record AzNextOptions : AzOptions
{
    [BooleanCommandSwitch("--command")]
    public bool? Command { get; set; }

    [BooleanCommandSwitch("--scenario")]
    public bool? Scenario { get; set; }
}