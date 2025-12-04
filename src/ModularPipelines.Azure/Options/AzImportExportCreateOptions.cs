using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("import-export", "create")]
public record AzImportExportCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--backup-drive-manifest")]
    public bool? BackupDriveManifest { get; set; }

    [CliFlag("--cancel-requested")]
    public bool? CancelRequested { get; set; }

    [CliOption("--client-tenant-id")]
    public string? ClientTenantId { get; set; }

    [CliOption("--delivery-package")]
    public string? DeliveryPackage { get; set; }

    [CliOption("--diagnostics-path")]
    public string? DiagnosticsPath { get; set; }

    [CliOption("--drive-list")]
    public string? DriveList { get; set; }

    [CliOption("--export")]
    public string? Export { get; set; }

    [CliOption("--incomplete-blob-list-uri")]
    public string? IncompleteBlobListUri { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--log-level")]
    public string? LogLevel { get; set; }

    [CliOption("--percent-complete")]
    public string? PercentComplete { get; set; }

    [CliOption("--return-address")]
    public string? ReturnAddress { get; set; }

    [CliOption("--return-package")]
    public string? ReturnPackage { get; set; }

    [CliOption("--return-shipping")]
    public string? ReturnShipping { get; set; }

    [CliOption("--shipping-information")]
    public string? ShippingInformation { get; set; }

    [CliOption("--state")]
    public string? State { get; set; }

    [CliOption("--storage-account")]
    public int? StorageAccount { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }
}