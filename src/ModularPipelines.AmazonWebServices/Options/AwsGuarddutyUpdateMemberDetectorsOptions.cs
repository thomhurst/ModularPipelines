using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("guardduty", "update-member-detectors")]
public record AwsGuarddutyUpdateMemberDetectorsOptions(
[property: CommandSwitch("--detector-id")] string DetectorId,
[property: CommandSwitch("--account-ids")] string[] AccountIds
) : AwsOptions
{
    [CommandSwitch("--data-sources")]
    public string? DataSources { get; set; }

    [CommandSwitch("--features")]
    public string[]? Features { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}