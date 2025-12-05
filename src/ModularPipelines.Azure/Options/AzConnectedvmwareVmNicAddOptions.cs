using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("connectedvmware", "vm", "nic", "add")]
public record AzConnectedvmwareVmNicAddOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--network")] string Network,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vm-name")] string VmName
) : AzOptions
{
    [CliOption("--nic-type")]
    public string? NicType { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--power-on-boot")]
    public string? PowerOnBoot { get; set; }
}