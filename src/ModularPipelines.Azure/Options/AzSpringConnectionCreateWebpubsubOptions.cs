using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("spring", "connection", "create", "webpubsub")]
public record AzSpringConnectionCreateWebpubsubOptions : AzOptions
{
    [CliOption("--app")]
    public string? App { get; set; }

    [CliOption("--client-type")]
    public string? ClientType { get; set; }

    [CliOption("--connection")]
    public string? Connection { get; set; }

    [CliOption("--customized-keys")]
    public string? CustomizedKeys { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliFlag("--private-endpoint")]
    public bool? PrivateEndpoint { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--secret")]
    public string? Secret { get; set; }

    [CliOption("--service")]
    public string? Service { get; set; }

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

    [CliOption("--webpubsub")]
    public string? Webpubsub { get; set; }
}