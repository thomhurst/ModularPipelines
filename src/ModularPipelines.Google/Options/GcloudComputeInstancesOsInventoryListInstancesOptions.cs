using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "os-inventory", "list-instances")]
public record GcloudComputeInstancesOsInventoryListInstancesOptions : GcloudOptions
{
    [CliOption("--inventory-filter")]
    public string? InventoryFilter { get; set; }

    [CliOption("--kernel-version")]
    public string? KernelVersion { get; set; }

    [CliOption("--os-shortname")]
    public string? OsShortname { get; set; }

    [CliOption("--os-version")]
    public string? OsVersion { get; set; }

    [CliOption("--package-name")]
    public string? PackageName { get; set; }

    [CliOption("--package-version")]
    public string? PackageVersion { get; set; }
}