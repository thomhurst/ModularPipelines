using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vm", "host", "create")]
public record AzVmHostCreateOptions(
[property: CommandSwitch("--host-group")] string HostGroup,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--sku")] string Sku
) : AzOptions
{
    [BooleanCommandSwitch("--auto-replace")]
    public bool? AutoReplace { get; set; }

    [CommandSwitch("--license-type")]
    public string? LicenseType { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--platform-fault-domain")]
    public string? PlatformFaultDomain { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}