using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("csvmware", "vm", "create")]
public record AzCsvmwareVmCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--private-cloud")] string PrivateCloud,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--resource-pool")] string ResourcePool,
[property: CommandSwitch("--template")] string Template
) : AzOptions
{
    [CommandSwitch("--cores")]
    public string? Cores { get; set; }

    [CommandSwitch("--disk")]
    public string? Disk { get; set; }

    [BooleanCommandSwitch("--expose-to-guest-vm")]
    public bool? ExposeToGuestVm { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--nic")]
    public string? Nic { get; set; }

    [CommandSwitch("--ram")]
    public string? Ram { get; set; }
}