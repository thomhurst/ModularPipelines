using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "connection", "update", "sql")]
public record AzWebappConnectionUpdateSqlOptions : AzOptions
{
    [CommandSwitch("--client-type")]
    public string? ClientType { get; set; }

    [BooleanCommandSwitch("--config-connstr")]
    public bool? ConfigConnstr { get; set; }

    [CommandSwitch("--connection")]
    public string? Connection { get; set; }

    [CommandSwitch("--customized-keys")]
    public string? CustomizedKeys { get; set; }

    [CommandSwitch("--id")]
    public string? Id { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [BooleanCommandSwitch("--private-endpoint")]
    public bool? PrivateEndpoint { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--secret")]
    public string? Secret { get; set; }

    [BooleanCommandSwitch("--service-endpoint")]
    public bool? ServiceEndpoint { get; set; }

    [CommandSwitch("--slot")]
    public string? Slot { get; set; }

    [CommandSwitch("--system-identity")]
    public string? SystemIdentity { get; set; }

    [CommandSwitch("--vault-id")]
    public string? VaultId { get; set; }
}