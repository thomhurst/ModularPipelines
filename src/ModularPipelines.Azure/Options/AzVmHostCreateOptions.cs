using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vm", "host", "create")]
public record AzVmHostCreateOptions(
[property: CliOption("--host-group")] string HostGroup,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--sku")] string Sku
) : AzOptions
{
    [CliFlag("--auto-replace")]
    public bool? AutoReplace { get; set; }

    [CliOption("--license-type")]
    public string? LicenseType { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--platform-fault-domain")]
    public string? PlatformFaultDomain { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}