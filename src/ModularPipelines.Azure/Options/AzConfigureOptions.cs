using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configure")]
public record AzConfigureOptions : AzOptions
{
    [CommandSwitch("--defaults")]
    public string? Defaults { get; set; }

    [BooleanCommandSwitch("--list-defaults")]
    public bool? ListDefaults { get; set; }

    [CommandSwitch("--scope")]
    public string? Scope { get; set; }
}