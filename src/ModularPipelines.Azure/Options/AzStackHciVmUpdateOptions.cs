using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("stack-hci-vm", "update")]
public record AzStackHciVmUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--computer-name")]
    public string? ComputerName { get; set; }

    [CliFlag("--enable-agent")]
    public bool? EnableAgent { get; set; }

    [CliFlag("--enable-vm-config-agent")]
    public bool? EnableVmConfigAgent { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--memory-mb")]
    public string? MemoryMb { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--nics")]
    public string? Nics { get; set; }

    [CliOption("--polling-interval")]
    public string? PollingInterval { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--v-cpus-available")]
    public string? VCpusAvailable { get; set; }

    [CliOption("--vhds")]
    public string? Vhds { get; set; }
}