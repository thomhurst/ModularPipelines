using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configservice", "put-retention-configuration")]
public record AwsConfigservicePutRetentionConfigurationOptions(
[property: CommandSwitch("--retention-period-in-days")] int RetentionPeriodInDays
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}