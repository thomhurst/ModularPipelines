using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("import-export", "show")]
public record AzImportExportShowOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--backup-drive-manifest")]
    public bool? BackupDriveManifest { get; set; }

    [BooleanCommandSwitch("--cancel-requested")]
    public bool? CancelRequested { get; set; }

    [CommandSwitch("--delivery-package")]
    public string? DeliveryPackage { get; set; }

    [CommandSwitch("--drive-list")]
    public string? DriveList { get; set; }

    [CommandSwitch("--log-level")]
    public string? LogLevel { get; set; }

    [CommandSwitch("--return-address")]
    public string? ReturnAddress { get; set; }

    [CommandSwitch("--return-shipping")]
    public string? ReturnShipping { get; set; }

    [CommandSwitch("--state")]
    public string? State { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}