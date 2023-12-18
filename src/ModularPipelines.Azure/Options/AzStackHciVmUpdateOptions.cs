using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stack-hci-vm", "update")]
public record AzStackHciVmUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--computer-name")]
    public string? ComputerName { get; set; }

    [BooleanCommandSwitch("--enable-agent")]
    public bool? EnableAgent { get; set; }

    [BooleanCommandSwitch("--enable-vm-config-agent")]
    public bool? EnableVmConfigAgent { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--memory-mb")]
    public string? MemoryMb { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--nics")]
    public string? Nics { get; set; }

    [CommandSwitch("--polling-interval")]
    public string? PollingInterval { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--v-cpus-available")]
    public string? VCpusAvailable { get; set; }

    [CommandSwitch("--vhds")]
    public string? Vhds { get; set; }
}