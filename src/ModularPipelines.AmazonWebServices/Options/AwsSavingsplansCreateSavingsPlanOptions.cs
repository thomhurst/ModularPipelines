using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("savingsplans", "create-savings-plan")]
public record AwsSavingsplansCreateSavingsPlanOptions(
[property: CliOption("--savings-plan-offering-id")] string SavingsPlanOfferingId,
[property: CliOption("--commitment")] string Commitment
) : AwsOptions
{
    [CliOption("--upfront-payment-amount")]
    public string? UpfrontPaymentAmount { get; set; }

    [CliOption("--purchase-time")]
    public long? PurchaseTime { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}