using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("containerapp", "connection", "create", "mysql-flexible", "(serviceconnector-passwordless", "extension)")]
public record AzContainerappConnectionCreateMysqlFlexibleServiceconnectorPasswordlessExtensionOptions : AzOptions
{
    [CliOption("--client-type")]
    public string? ClientType { get; set; }

    [CliOption("--connection")]
    public string? Connection { get; set; }

    [CliOption("--container")]
    public string? Container { get; set; }

    [CliOption("--customized-keys")]
    public string? CustomizedKeys { get; set; }

    [CliOption("--database")]
    public string? Database { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--secret")]
    public string? Secret { get; set; }

    [CliOption("--server")]
    public string? Server { get; set; }

    [CliOption("--service-principal")]
    public string? ServicePrincipal { get; set; }

    [CliOption("--source-id")]
    public string? SourceId { get; set; }

    [CliOption("--system-identity")]
    public string? SystemIdentity { get; set; }

    [CliOption("--target-id")]
    public string? TargetId { get; set; }

    [CliOption("--target-resource-group")]
    public string? TargetResourceGroup { get; set; }

    [CliOption("--user-identity")]
    public string? UserIdentity { get; set; }

    [CliOption("--vault-id")]
    public string? VaultId { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}