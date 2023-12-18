using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mariadb", "server", "vnet-rule", "create")]
public record AzMariadbServerVnetRuleCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--server-name")] string ServerName,
[property: CommandSwitch("--subnet")] string Subnet
) : AzOptions
{
    [BooleanCommandSwitch("--ignore-missing-endpoint")]
    public bool? IgnoreMissingEndpoint { get; set; }

    [CommandSwitch("--vnet-name")]
    public string? VnetName { get; set; }
}

