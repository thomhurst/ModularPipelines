using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("amlfs", "create")]
public record AzAmlfsCreateOptions(
[property: CommandSwitch("--aml-filesystem-name")] string AmlFilesystemName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--encryption-setting")]
    public string? EncryptionSetting { get; set; }

    [CommandSwitch("--filesystem-subnet")]
    public string? FilesystemSubnet { get; set; }

    [CommandSwitch("--hsm-settings")]
    public string? HsmSettings { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--maintenance-window")]
    public string? MaintenanceWindow { get; set; }

    [CommandSwitch("--mi-user-assigned")]
    public string? MiUserAssigned { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--storage-capacity")]
    public string? StorageCapacity { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--zones")]
    public string? Zones { get; set; }
}

