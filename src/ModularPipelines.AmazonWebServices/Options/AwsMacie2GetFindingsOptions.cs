using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("macie2", "get-findings")]
public record AwsMacie2GetFindingsOptions(
[property: CommandSwitch("--finding-ids")] string[] FindingIds
) : AwsOptions
{
    [CommandSwitch("--sort-criteria")]
    public string? SortCriteria { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}