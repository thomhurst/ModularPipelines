using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("disk-encryption-set", "update")]
public record AzDiskEncryptionSetUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliFlag("--auto-rotation")]
    public bool? AutoRotation { get; set; }

    [CliOption("--federated-client-id")]
    public string? FederatedClientId { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--key-url")]
    public string? KeyUrl { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--source-vault")]
    public string? SourceVault { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}