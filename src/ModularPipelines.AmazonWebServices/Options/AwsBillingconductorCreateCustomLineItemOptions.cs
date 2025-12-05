using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billingconductor", "create-custom-line-item")]
public record AwsBillingconductorCreateCustomLineItemOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--description")] string Description,
[property: CliOption("--billing-group-arn")] string BillingGroupArn,
[property: CliOption("--charge-details")] string ChargeDetails
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--billing-period-range")]
    public string? BillingPeriodRange { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--account-id")]
    public string? AccountId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}