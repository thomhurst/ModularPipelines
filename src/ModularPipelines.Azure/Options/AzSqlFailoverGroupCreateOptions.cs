using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "failover-group", "create")]
public record AzSqlFailoverGroupCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--partner-server")] string PartnerServer,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--server")] string Server
) : AzOptions
{
    [CommandSwitch("--add-db")]
    public string? AddDb { get; set; }

    [CommandSwitch("--failover-policy")]
    public string? FailoverPolicy { get; set; }

    [CommandSwitch("--grace-period")]
    public string? GracePeriod { get; set; }

    [CommandSwitch("--partner-resource-group")]
    public string? PartnerResourceGroup { get; set; }
}