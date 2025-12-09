using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("arcappliance", "update-infracredentials", "vmware")]
public record AzArcapplianceUpdateInfracredentialsVmwareOptions(
[property: CliOption("--kubeconfig")] string Kubeconfig
) : AzOptions
{
    [CliOption("--address")]
    public string? Address { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliFlag("--skipWait")]
    public bool? SkipWait { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }
}