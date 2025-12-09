using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "update-billing-group")]
public record AwsIotUpdateBillingGroupOptions(
[property: CliOption("--billing-group-name")] string BillingGroupName,
[property: CliOption("--billing-group-properties")] string BillingGroupProperties
) : AwsOptions
{
    [CliOption("--expected-version")]
    public long? ExpectedVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}