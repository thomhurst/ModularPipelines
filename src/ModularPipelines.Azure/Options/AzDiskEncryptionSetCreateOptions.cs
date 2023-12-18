using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("disk-encryption-set", "create")]
public record AzDiskEncryptionSetCreateOptions(
[property: CommandSwitch("--key-url")] string KeyUrl,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--auto-rotation")]
    public bool? AutoRotation { get; set; }

    [CommandSwitch("--encryption-type")]
    public string? EncryptionType { get; set; }

    [CommandSwitch("--federated-client-id")]
    public string? FederatedClientId { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--mi-system-assigned")]
    public bool? MiSystemAssigned { get; set; }

    [CommandSwitch("--mi-user-assigned")]
    public string? MiUserAssigned { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--source-vault")]
    public string? SourceVault { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}