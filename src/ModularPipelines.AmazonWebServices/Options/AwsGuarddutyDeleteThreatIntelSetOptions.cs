using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("guardduty", "delete-threat-intel-set")]
public record AwsGuarddutyDeleteThreatIntelSetOptions(
[property: CommandSwitch("--detector-id")] string DetectorId,
[property: CommandSwitch("--threat-intel-set-id")] string ThreatIntelSetId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}