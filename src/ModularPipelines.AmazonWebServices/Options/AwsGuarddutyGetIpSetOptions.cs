using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("guardduty", "get-ip-set")]
public record AwsGuarddutyGetIpSetOptions(
[property: CliOption("--detector-id")] string DetectorId,
[property: CliOption("--ip-set-id")] string IpSetId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}