using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("macie2", "get-usage-totals")]
public record AwsMacie2GetUsageTotalsOptions : AwsOptions
{
    [CommandSwitch("--time-range")]
    public string? TimeRange { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}