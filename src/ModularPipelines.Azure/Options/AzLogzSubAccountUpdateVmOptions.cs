using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logz", "sub-account", "update-vm")]
public record AzLogzSubAccountUpdateVmOptions : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--monitor-name")]
    public string? MonitorName { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--state")]
    public string? State { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--vm-resource-ids")]
    public string? VmResourceIds { get; set; }
}