using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing", "invoice", "section", "wait")]
public record AzBillingInvoiceSectionWaitOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--invoice-section-name")] string InvoiceSectionName,
[property: CliOption("--profile-name")] string ProfileName
) : AzOptions
{
    [CliFlag("--created")]
    public bool? Created { get; set; }

    [CliOption("--custom")]
    public string? Custom { get; set; }

    [CliFlag("--deleted")]
    public bool? Deleted { get; set; }

    [CliFlag("--exists")]
    public bool? Exists { get; set; }

    [CliOption("--interval")]
    public int? Interval { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliFlag("--updated")]
    public bool? Updated { get; set; }
}