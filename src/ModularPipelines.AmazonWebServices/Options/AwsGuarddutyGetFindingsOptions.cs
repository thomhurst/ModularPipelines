using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("guardduty", "get-findings")]
public record AwsGuarddutyGetFindingsOptions(
[property: CommandSwitch("--detector-id")] string DetectorId,
[property: CommandSwitch("--finding-ids")] string[] FindingIds
) : AwsOptions
{
    [CommandSwitch("--sort-criteria")]
    public string? SortCriteria { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}