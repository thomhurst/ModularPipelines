using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "vm", "group", "ag-listener", "create")]
public record AzSqlVmGroupAgListenerCreateOptions(
[property: CommandSwitch("--ag-name")] string AgName,
[property: CommandSwitch("--group-name")] string GroupName,
[property: CommandSwitch("--ip-address")] string IpAddress,
[property: CommandSwitch("--load-balancer")] string LoadBalancer,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--probe-port")] string ProbePort,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--sqlvms")] string Sqlvms,
[property: CommandSwitch("--subnet")] string Subnet
) : AzOptions
{
    [CommandSwitch("--port")]
    public int? Port { get; set; }

    [CommandSwitch("--public-ip")]
    public string? PublicIp { get; set; }

    [CommandSwitch("--vnet-name")]
    public string? VnetName { get; set; }
}