using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arcappliance", "update-infracredentials", "vmware")]
public record AzArcapplianceUpdateInfracredentialsVmwareOptions(
[property: CommandSwitch("--kubeconfig")] string Kubeconfig
) : AzOptions
{
    [CommandSwitch("--address")]
    public string? Address { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [BooleanCommandSwitch("--skipWait")]
    public bool? SkipWait { get; set; }

    [CommandSwitch("--username")]
    public string? Username { get; set; }
}