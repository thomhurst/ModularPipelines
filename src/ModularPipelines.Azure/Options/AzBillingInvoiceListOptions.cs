using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing", "invoice", "list")]
public record AzBillingInvoiceListOptions(
[property: CliOption("--period-end-date")] string PeriodEndDate,
[property: CliOption("--period-start-date")] string PeriodStartDate
) : AzOptions
{
    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--profile-name")]
    public string? ProfileName { get; set; }
}