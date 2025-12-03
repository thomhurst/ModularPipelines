using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectedvmware", "vm", "nic", "update")]
public record AzConnectedvmwareVmNicUpdateOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vm-name")] string VmName
) : AzOptions
{
    [CliOption("--device-key")]
    public string? DeviceKey { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--power-on-boot")]
    public string? PowerOnBoot { get; set; }
}