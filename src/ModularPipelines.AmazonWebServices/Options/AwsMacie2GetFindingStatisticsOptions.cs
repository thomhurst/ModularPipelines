using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("macie2", "get-finding-statistics")]
public record AwsMacie2GetFindingStatisticsOptions(
[property: CommandSwitch("--group-by")] string GroupBy
) : AwsOptions
{
    [CommandSwitch("--finding-criteria")]
    public string? FindingCriteria { get; set; }

    [CommandSwitch("--size")]
    public int? Size { get; set; }

    [CommandSwitch("--sort-criteria")]
    public string? SortCriteria { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}