using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billingconductor", "batch-disassociate-resources-from-custom-line-item")]
public record AwsBillingconductorBatchDisassociateResourcesFromCustomLineItemOptions(
[property: CliOption("--target-arn")] string TargetArn,
[property: CliOption("--resource-arns")] string[] ResourceArns
) : AwsOptions
{
    [CliOption("--billing-period-range")]
    public string? BillingPeriodRange { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}