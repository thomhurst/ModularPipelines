using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotevents-data", "list-detectors")]
public record AwsIoteventsDataListDetectorsOptions(
[property: CliOption("--detector-model-name")] string DetectorModelName
) : AwsOptions
{
    [CliOption("--state-name")]
    public string? StateName { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}