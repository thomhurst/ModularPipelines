using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("arcappliance", "update-infracredentials", "scvmm")]
public record AzArcapplianceUpdateInfracredentialsScvmmOptions(
[property: CliOption("--kubeconfig")] string Kubeconfig
) : AzOptions
{
    [CliOption("--address")]
    public string? Address { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }
}