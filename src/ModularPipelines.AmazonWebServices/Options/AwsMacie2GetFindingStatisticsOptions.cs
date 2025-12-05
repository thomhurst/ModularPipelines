using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("macie2", "get-finding-statistics")]
public record AwsMacie2GetFindingStatisticsOptions(
[property: CliOption("--group-by")] string GroupBy
) : AwsOptions
{
    [CliOption("--finding-criteria")]
    public string? FindingCriteria { get; set; }

    [CliOption("--size")]
    public int? Size { get; set; }

    [CliOption("--sort-criteria")]
    public string? SortCriteria { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}