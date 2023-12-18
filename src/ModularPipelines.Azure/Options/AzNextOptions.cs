using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("next")]
public record AzNextOptions(
[property: CommandSwitch("--uri")] string Uri
) : AzOptions
{
    [BooleanCommandSwitch("--command")]
    public bool? Command { get; set; }

    [BooleanCommandSwitch("--scenario")]
    public bool? Scenario { get; set; }
}

