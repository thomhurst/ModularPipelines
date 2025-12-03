using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billing-benefits", "savings-plan-order-aliases", "create")]
public record AzBillingBenefitsSavingsPlanOrderAliasesCreateOptions(
[property: CliOption("--order-alias-name")] string OrderAliasName
) : AzOptions
{
    [CliOption("--applied-scope-prop")]
    public string? AppliedScopeProp { get; set; }

    [CliOption("--applied-scope-type")]
    public string? AppliedScopeType { get; set; }

    [CliOption("--billing-plan")]
    public string? BillingPlan { get; set; }

    [CliOption("--billing-scope-id")]
    public string? BillingScopeId { get; set; }

    [CliOption("--commitment")]
    public string? Commitment { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--term")]
    public string? Term { get; set; }
}