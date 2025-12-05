using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("macie2", "get-findings")]
public record AwsMacie2GetFindingsOptions(
[property: CliOption("--finding-ids")] string[] FindingIds
) : AwsOptions
{
    [CliOption("--sort-criteria")]
    public string? SortCriteria { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}