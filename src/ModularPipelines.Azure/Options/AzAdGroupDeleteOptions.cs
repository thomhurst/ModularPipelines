using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "group", "delete")]
public record AzAdGroupDeleteOptions(
[property: CommandSwitch("--group")] string Group
) : AzOptions
{
    [BooleanCommandSwitch("--security-enabled-only")]
    public bool? SecurityEnabledOnly { get; set; }
}

