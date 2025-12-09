using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("mysql", "server", "vnet-rule", "update")]
public record AzMysqlServerVnetRuleUpdateOptions(
[property: CliOption("--subnet")] string Subnet
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--ignore-missing-endpoint")]
    public bool? IgnoreMissingEndpoint { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--server-name")]
    public string? ServerName { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--vnet-name")]
    public string? VnetName { get; set; }
}