using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup", "vault", "encryption", "update")]
public record AzBackupVaultEncryptionUpdateOptions(
[property: CliOption("--encryption-key-id")] string EncryptionKeyId
) : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--infrastructure-encryption")]
    public string? InfrastructureEncryption { get; set; }

    [CliOption("--mi-system-assigned")]
    public string? MiSystemAssigned { get; set; }

    [CliOption("--mi-user-assigned")]
    public string? MiUserAssigned { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}