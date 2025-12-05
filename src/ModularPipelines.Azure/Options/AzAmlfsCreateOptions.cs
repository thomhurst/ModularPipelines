using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("amlfs", "create")]
public record AzAmlfsCreateOptions(
[property: CliOption("--aml-filesystem-name")] string AmlFilesystemName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--encryption-setting")]
    public string? EncryptionSetting { get; set; }

    [CliOption("--filesystem-subnet")]
    public string? FilesystemSubnet { get; set; }

    [CliOption("--hsm-settings")]
    public string? HsmSettings { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--maintenance-window")]
    public string? MaintenanceWindow { get; set; }

    [CliOption("--mi-user-assigned")]
    public string? MiUserAssigned { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--storage-capacity")]
    public string? StorageCapacity { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--zones")]
    public string? Zones { get; set; }
}