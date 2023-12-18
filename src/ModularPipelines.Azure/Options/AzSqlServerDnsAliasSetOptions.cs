using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "server", "dns-alias", "set")]
public record AzSqlServerDnsAliasSetOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--original-server")] string OriginalServer
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--original-resource-group")]
    public string? OriginalResourceGroup { get; set; }

    [CommandSwitch("--original-subscription-id")]
    public string? OriginalSubscriptionId { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--server")]
    public string? Server { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}