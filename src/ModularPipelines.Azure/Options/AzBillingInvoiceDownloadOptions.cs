using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "invoice", "download")]
public record AzBillingInvoiceDownloadOptions : AzOptions
{
    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--download-token")]
    public string? DownloadToken { get; set; }

    [CommandSwitch("--download-urls")]
    public string? DownloadUrls { get; set; }

    [CommandSwitch("--invoice-name")]
    public string? InvoiceName { get; set; }
}