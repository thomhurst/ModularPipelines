using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billingconductor", "get-billing-group-cost-report")]
public record AwsBillingconductorGetBillingGroupCostReportOptions(
[property: CliOption("--arn")] string Arn
) : AwsOptions
{
    [CliOption("--billing-period-range")]
    public string? BillingPeriodRange { get; set; }

    [CliOption("--group-by")]
    public string[]? GroupBy { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}