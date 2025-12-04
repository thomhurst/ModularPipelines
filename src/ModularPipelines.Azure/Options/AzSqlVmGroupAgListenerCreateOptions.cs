using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "vm", "group", "ag-listener", "create")]
public record AzSqlVmGroupAgListenerCreateOptions(
[property: CliOption("--ag-name")] string AgName,
[property: CliOption("--group-name")] string GroupName,
[property: CliOption("--ip-address")] string IpAddress,
[property: CliOption("--load-balancer")] string LoadBalancer,
[property: CliOption("--name")] string Name,
[property: CliOption("--probe-port")] string ProbePort,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--sqlvms")] string Sqlvms,
[property: CliOption("--subnet")] string Subnet
) : AzOptions
{
    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--public-ip")]
    public string? PublicIp { get; set; }

    [CliOption("--vnet-name")]
    public string? VnetName { get; set; }
}