using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billingconductor", "batch-associate-resources-to-custom-line-item")]
public record AwsBillingconductorBatchAssociateResourcesToCustomLineItemOptions(
[property: CommandSwitch("--target-arn")] string TargetArn,
[property: CommandSwitch("--resource-arns")] string[] ResourceArns
) : AwsOptions
{
    [CommandSwitch("--billing-period-range")]
    public string? BillingPeriodRange { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}