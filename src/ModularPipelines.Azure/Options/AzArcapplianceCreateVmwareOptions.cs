using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("arcappliance", "create", "vmware")]
public record AzArcapplianceCreateVmwareOptions(
[property: CommandSwitch("--config-file")] string ConfigFile,
[property: CommandSwitch("--kubeconfig")] string Kubeconfig
) : AzOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}