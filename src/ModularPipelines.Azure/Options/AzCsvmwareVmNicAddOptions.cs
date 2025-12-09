using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("csvmware", "vm", "nic", "add")]
public record AzCsvmwareVmNicAddOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--virtual-network")] string VirtualNetwork,
[property: CliOption("--vm-name")] string VmName
) : AzOptions
{
    [CliOption("--adapter")]
    public string? Adapter { get; set; }

    [CliFlag("--power-on-boot")]
    public bool? PowerOnBoot { get; set; }
}