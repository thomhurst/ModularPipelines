using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "create-billing-group")]
public record AwsIotCreateBillingGroupOptions(
[property: CliOption("--billing-group-name")] string BillingGroupName
) : AwsOptions
{
    [CliOption("--billing-group-properties")]
    public string? BillingGroupProperties { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}