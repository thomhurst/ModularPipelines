using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "server", "vnet-rule", "create")]
public record AzSqlServerVnetRuleCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--server")] string Server,
[property: CliOption("--subnet")] string Subnet
) : AzOptions
{
    [CliFlag("--ignore-missing-endpoint")]
    public bool? IgnoreMissingEndpoint { get; set; }

    [CliOption("--vnet-name")]
    public string? VnetName { get; set; }
}