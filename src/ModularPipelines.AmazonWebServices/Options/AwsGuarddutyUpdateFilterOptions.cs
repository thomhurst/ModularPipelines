using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("guardduty", "update-filter")]
public record AwsGuarddutyUpdateFilterOptions(
[property: CommandSwitch("--detector-id")] string DetectorId,
[property: CommandSwitch("--filter-name")] string FilterName
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--action")]
    public string? Action { get; set; }

    [CommandSwitch("--rank")]
    public int? Rank { get; set; }

    [CommandSwitch("--finding-criteria")]
    public string? FindingCriteria { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}