using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("disk-encryption-set", "create")]
public record AzDiskEncryptionSetCreateOptions(
[property: CliOption("--key-url")] string KeyUrl,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--auto-rotation")]
    public bool? AutoRotation { get; set; }

    [CliOption("--encryption-type")]
    public string? EncryptionType { get; set; }

    [CliOption("--federated-client-id")]
    public string? FederatedClientId { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--mi-system-assigned")]
    public bool? MiSystemAssigned { get; set; }

    [CliOption("--mi-user-assigned")]
    public string? MiUserAssigned { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--source-vault")]
    public string? SourceVault { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}