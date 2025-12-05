using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("workloads", "sap-virtual-instance", "create")]
public record AzWorkloadsSapVirtualInstanceCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--central-server-vm")]
    public string? CentralServerVm { get; set; }

    [CliOption("--configuration")]
    public string? Configuration { get; set; }

    [CliOption("--environment")]
    public string? Environment { get; set; }

    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--managed-rg-name")]
    public string? ManagedRgName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--sap-product")]
    public string? SapProduct { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}