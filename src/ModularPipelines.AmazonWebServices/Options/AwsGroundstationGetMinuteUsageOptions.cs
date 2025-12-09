using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("groundstation", "get-minute-usage")]
public record AwsGroundstationGetMinuteUsageOptions(
[property: CliOption("--month")] int Month,
[property: CliOption("--year")] int Year
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}