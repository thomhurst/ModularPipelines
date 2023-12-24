using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("guardduty", "get-ip-set")]
public record AwsGuarddutyGetIpSetOptions(
[property: CommandSwitch("--detector-id")] string DetectorId,
[property: CommandSwitch("--ip-set-id")] string IpSetId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}