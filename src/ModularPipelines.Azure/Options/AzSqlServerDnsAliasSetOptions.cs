using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "server", "dns-alias", "set")]
public record AzSqlServerDnsAliasSetOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--original-server")] string OriginalServer
) : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--original-resource-group")]
    public string? OriginalResourceGroup { get; set; }

    [CliOption("--original-subscription-id")]
    public string? OriginalSubscriptionId { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--server")]
    public string? Server { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}