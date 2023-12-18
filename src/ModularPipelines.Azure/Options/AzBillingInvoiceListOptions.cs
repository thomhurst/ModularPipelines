using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "invoice", "list")]
public record AzBillingInvoiceListOptions(
[property: CommandSwitch("--period-end-date")] string PeriodEndDate,
[property: CommandSwitch("--period-start-date")] string PeriodStartDate
) : AzOptions
{
    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--profile-name")]
    public string? ProfileName { get; set; }
}