using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vm", "list-skus")]
public record AzVmListSkusOptions : AzOptions
{
    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--resource-type")]
    public string? ResourceType { get; set; }

    [CommandSwitch("--size")]
    public string? Size { get; set; }

    [BooleanCommandSwitch("--zone")]
    public bool? Zone { get; set; }
}