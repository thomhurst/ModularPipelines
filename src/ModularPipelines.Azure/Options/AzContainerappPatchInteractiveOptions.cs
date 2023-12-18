using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("containerapp", "patch", "interactive")]
public record AzContainerappPatchInteractiveOptions : AzOptions
{
    [CommandSwitch("--environment")]
    public string? Environment { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [BooleanCommandSwitch("--show-all")]
    public bool? ShowAll { get; set; }
}