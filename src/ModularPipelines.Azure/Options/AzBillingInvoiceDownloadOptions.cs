using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("billing", "invoice", "download")]
public record AzBillingInvoiceDownloadOptions : AzOptions
{
    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--download-token")]
    public string? DownloadToken { get; set; }

    [CliOption("--download-urls")]
    public string? DownloadUrls { get; set; }

    [CliOption("--invoice-name")]
    public string? InvoiceName { get; set; }
}