using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("groundstation", "get-minute-usage")]
public record AwsGroundstationGetMinuteUsageOptions(
[property: CommandSwitch("--month")] int Month,
[property: CommandSwitch("--year")] int Year
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}