using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("csvmware", "vm", "create")]
public record AzCsvmwareVmCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--private-cloud")] string PrivateCloud,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--resource-pool")] string ResourcePool,
[property: CliOption("--template")] string Template
) : AzOptions
{
    [CliOption("--cores")]
    public string? Cores { get; set; }

    [CliOption("--disk")]
    public string? Disk { get; set; }

    [CliFlag("--expose-to-guest-vm")]
    public bool? ExposeToGuestVm { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--nic")]
    public string? Nic { get; set; }

    [CliOption("--ram")]
    public string? Ram { get; set; }
}