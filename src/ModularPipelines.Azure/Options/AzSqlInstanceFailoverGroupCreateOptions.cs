using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "instance-failover-group", "create")]
public record AzSqlInstanceFailoverGroupCreateOptions(
[property: CommandSwitch("--mi")] string Mi,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--partner-mi")] string PartnerMi,
[property: CommandSwitch("--partner-resource-group")] string PartnerResourceGroup,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--failover-policy")]
    public string? FailoverPolicy { get; set; }

    [CommandSwitch("--grace-period")]
    public string? GracePeriod { get; set; }

    [CommandSwitch("--secondary-type")]
    public string? SecondaryType { get; set; }
}