using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("containerapp", "connection", "update", "keyvault")]
public record AzContainerappConnectionUpdateKeyvaultOptions : AzOptions
{
    [CliOption("--client-type")]
    public string? ClientType { get; set; }

    [CliOption("--connection")]
    public string? Connection { get; set; }

    [CliOption("--container")]
    public string? Container { get; set; }

    [CliOption("--customized-keys")]
    public string? CustomizedKeys { get; set; }

    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliFlag("--private-endpoint")]
    public bool? PrivateEndpoint { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--service-endpoint")]
    public bool? ServiceEndpoint { get; set; }

    [CliOption("--service-principal")]
    public string? ServicePrincipal { get; set; }

    [CliOption("--system-identity")]
    public string? SystemIdentity { get; set; }

    [CliOption("--user-identity")]
    public string? UserIdentity { get; set; }

    [CliOption("--vault-id")]
    public string? VaultId { get; set; }
}