using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billingconductor", "update-billing-group")]
public record AwsBillingconductorUpdateBillingGroupOptions(
[property: CliOption("--arn")] string Arn
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--computation-preference")]
    public string? ComputationPreference { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--account-grouping")]
    public string? AccountGrouping { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}