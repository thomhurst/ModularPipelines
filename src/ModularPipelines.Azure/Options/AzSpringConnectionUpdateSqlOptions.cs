using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring", "connection", "update", "sql")]
public record AzSpringConnectionUpdateSqlOptions : AzOptions
{
    [CommandSwitch("--app")]
    public string? App { get; set; }

    [CommandSwitch("--client-type")]
    public string? ClientType { get; set; }

    [CommandSwitch("--connection")]
    public string? Connection { get; set; }

    [CommandSwitch("--customized-keys")]
    public string? CustomizedKeys { get; set; }

    [CommandSwitch("--id")]
    public string? Id { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [BooleanCommandSwitch("--private-endpoint")]
    public bool? PrivateEndpoint { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--secret")]
    public string? Secret { get; set; }

    [CommandSwitch("--service")]
    public string? Service { get; set; }

    [BooleanCommandSwitch("--service-endpoint")]
    public bool? ServiceEndpoint { get; set; }

    [CommandSwitch("--system-identity")]
    public string? SystemIdentity { get; set; }

    [CommandSwitch("--vault-id")]
    public string? VaultId { get; set; }
}