using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("arcappliance", "update-infracredentials", "hci")]
public record AzArcapplianceUpdateInfracredentialsHciOptions(
[property: CliOption("--kubeconfig")] string Kubeconfig
) : AzOptions
{
    [CliOption("--cloudagent")]
    public string? Cloudagent { get; set; }

    [CliOption("--loginconfigfile")]
    public string? Loginconfigfile { get; set; }
}