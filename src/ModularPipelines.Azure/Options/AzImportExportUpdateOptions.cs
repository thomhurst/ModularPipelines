using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("import-export", "update")]
public record AzImportExportUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--backup-drive-manifest")]
    public bool? BackupDriveManifest { get; set; }

    [CliFlag("--cancel-requested")]
    public bool? CancelRequested { get; set; }

    [CliOption("--delivery-package")]
    public string? DeliveryPackage { get; set; }

    [CliOption("--drive-list")]
    public string? DriveList { get; set; }

    [CliOption("--log-level")]
    public string? LogLevel { get; set; }

    [CliOption("--return-address")]
    public string? ReturnAddress { get; set; }

    [CliOption("--return-shipping")]
    public string? ReturnShipping { get; set; }

    [CliOption("--state")]
    public string? State { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}