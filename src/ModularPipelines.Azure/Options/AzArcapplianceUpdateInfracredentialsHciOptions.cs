using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arcappliance", "update-infracredentials", "hci")]
public record AzArcapplianceUpdateInfracredentialsHciOptions(
[property: CommandSwitch("--kubeconfig")] string Kubeconfig
) : AzOptions
{
    [CommandSwitch("--cloudagent")]
    public string? Cloudagent { get; set; }

    [CommandSwitch("--loginconfigfile")]
    public string? Loginconfigfile { get; set; }
}