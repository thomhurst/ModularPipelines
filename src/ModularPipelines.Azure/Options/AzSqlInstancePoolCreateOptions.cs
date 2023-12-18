using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "instance-pool", "create")]
public record AzSqlInstancePoolCreateOptions(
[property: CommandSwitch("--capacity")] string Capacity,
[property: CommandSwitch("--edition")] string Edition,
[property: CommandSwitch("--family")] string Family,
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--subnet")] string Subnet
) : AzOptions
{
    [CommandSwitch("--license-type")]
    public string? LicenseType { get; set; }

    [CommandSwitch("--maint-config-id")]
    public string? MaintConfigId { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--vnet-name")]
    public string? VnetName { get; set; }
}