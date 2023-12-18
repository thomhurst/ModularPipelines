using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("import-export", "create")]
public record AzImportExportCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--backup-drive-manifest")]
    public bool? BackupDriveManifest { get; set; }

    [BooleanCommandSwitch("--cancel-requested")]
    public bool? CancelRequested { get; set; }

    [CommandSwitch("--client-tenant-id")]
    public string? ClientTenantId { get; set; }

    [CommandSwitch("--delivery-package")]
    public string? DeliveryPackage { get; set; }

    [CommandSwitch("--diagnostics-path")]
    public string? DiagnosticsPath { get; set; }

    [CommandSwitch("--drive-list")]
    public string? DriveList { get; set; }

    [CommandSwitch("--export")]
    public string? Export { get; set; }

    [CommandSwitch("--incomplete-blob-list-uri")]
    public string? IncompleteBlobListUri { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--log-level")]
    public string? LogLevel { get; set; }

    [CommandSwitch("--percent-complete")]
    public string? PercentComplete { get; set; }

    [CommandSwitch("--return-address")]
    public string? ReturnAddress { get; set; }

    [CommandSwitch("--return-package")]
    public string? ReturnPackage { get; set; }

    [CommandSwitch("--return-shipping")]
    public string? ReturnShipping { get; set; }

    [CommandSwitch("--shipping-information")]
    public string? ShippingInformation { get; set; }

    [CommandSwitch("--state")]
    public string? State { get; set; }

    [CommandSwitch("--storage-account")]
    public int? StorageAccount { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }
}