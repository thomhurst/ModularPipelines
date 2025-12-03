using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "put-retention-configuration")]
public record AwsConfigservicePutRetentionConfigurationOptions(
[property: CliOption("--retention-period-in-days")] int RetentionPeriodInDays
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}