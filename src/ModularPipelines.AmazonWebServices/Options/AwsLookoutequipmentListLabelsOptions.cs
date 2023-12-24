using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutequipment", "list-labels")]
public record AwsLookoutequipmentListLabelsOptions(
[property: CommandSwitch("--label-group-name")] string LabelGroupName
) : AwsOptions
{
    [CommandSwitch("--interval-start-time")]
    public long? IntervalStartTime { get; set; }

    [CommandSwitch("--interval-end-time")]
    public long? IntervalEndTime { get; set; }

    [CommandSwitch("--fault-code")]
    public string? FaultCode { get; set; }

    [CommandSwitch("--equipment")]
    public string? Equipment { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}