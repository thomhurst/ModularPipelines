using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billingconductor", "update-custom-line-item")]
public record AwsBillingconductorUpdateCustomLineItemOptions(
[property: CliOption("--arn")] string Arn
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--charge-details")]
    public string? ChargeDetails { get; set; }

    [CliOption("--billing-period-range")]
    public string? BillingPeriodRange { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}