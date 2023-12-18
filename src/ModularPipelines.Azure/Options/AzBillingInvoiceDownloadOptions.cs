using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "invoice", "download")]
public record AzBillingInvoiceDownloadOptions(
[property: CommandSwitch("--period-end-date")] string PeriodEndDate,
[property: CommandSwitch("--period-start-date")] string PeriodStartDate
) : AzOptions
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