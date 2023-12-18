using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functionapp", "connection", "create", "signalr")]
public record AzFunctionappConnectionCreateSignalrOptions : AzOptions
{
    [CommandSwitch("--client-type")]
    public string? ClientType { get; set; }

    [CommandSwitch("--connection")]
    public string? Connection { get; set; }

    [CommandSwitch("--customized-keys")]
    public string? CustomizedKeys { get; set; }

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

    [CommandSwitch("--service-principal")]
    public string? ServicePrincipal { get; set; }

    [CommandSwitch("--signalr")]
    public string? Signalr { get; set; }

    [CommandSwitch("--source-id")]
    public string? SourceId { get; set; }

    [CommandSwitch("--system-identity")]
    public string? SystemIdentity { get; set; }

    [CommandSwitch("--target-id")]
    public string? TargetId { get; set; }

    [CommandSwitch("--target-resource-group")]
    public string? TargetResourceGroup { get; set; }

    [CommandSwitch("--user-identity")]
    public string? UserIdentity { get; set; }

    [CommandSwitch("--vault-id")]
    public string? VaultId { get; set; }
}