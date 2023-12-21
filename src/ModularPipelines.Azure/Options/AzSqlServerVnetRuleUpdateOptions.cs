using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "server", "vnet-rule", "update")]
public record AzSqlServerVnetRuleUpdateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--subnet")] string Subnet
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--ignore-missing-endpoint")]
    public bool? IgnoreMissingEndpoint { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--server")]
    public string? Server { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}