using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotevents-data", "list-detectors")]
public record AwsIoteventsDataListDetectorsOptions(
[property: CommandSwitch("--detector-model-name")] string DetectorModelName
) : AwsOptions
{
    [CommandSwitch("--state-name")]
    public string? StateName { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}