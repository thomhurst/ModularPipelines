using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vm", "run-command", "list")]
public record AzVmRunCommandListOptions : AzOptions
{
    [CommandSwitch("--expand")]
    public string? Expand { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--vm-name")]
    public string? VmName { get; set; }
}