using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("savingsplans", "create-savings-plan")]
public record AwsSavingsplansCreateSavingsPlanOptions(
[property: CommandSwitch("--savings-plan-offering-id")] string SavingsPlanOfferingId,
[property: CommandSwitch("--commitment")] string Commitment
) : AwsOptions
{
    [CommandSwitch("--upfront-payment-amount")]
    public string? UpfrontPaymentAmount { get; set; }

    [CommandSwitch("--purchase-time")]
    public long? PurchaseTime { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}