using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codeguru-security", "get-metrics-summary")]
public record AwsCodeguruSecurityGetMetricsSummaryOptions(
[property: CommandSwitch("--date")] long Date
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}