using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("guardduty", "archive-findings")]
public record AwsGuarddutyArchiveFindingsOptions(
[property: CommandSwitch("--detector-id")] string DetectorId,
[property: CommandSwitch("--finding-ids")] string[] FindingIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}