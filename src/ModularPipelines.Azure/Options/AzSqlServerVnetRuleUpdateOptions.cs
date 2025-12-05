using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "server", "vnet-rule", "update")]
public record AzSqlServerVnetRuleUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--subnet")] string Subnet
) : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--ignore-missing-endpoint")]
    public bool? IgnoreMissingEndpoint { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--server")]
    public string? Server { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}