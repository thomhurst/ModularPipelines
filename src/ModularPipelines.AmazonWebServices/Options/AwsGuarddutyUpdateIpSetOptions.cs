using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("guardduty", "update-ip-set")]
public record AwsGuarddutyUpdateIpSetOptions(
[property: CliOption("--detector-id")] string DetectorId,
[property: CliOption("--ip-set-id")] string IpSetId
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}