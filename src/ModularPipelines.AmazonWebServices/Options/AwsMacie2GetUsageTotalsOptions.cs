using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("macie2", "get-usage-totals")]
public record AwsMacie2GetUsageTotalsOptions : AwsOptions
{
    [CliOption("--time-range")]
    public string? TimeRange { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}