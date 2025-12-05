using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutequipment", "list-labels")]
public record AwsLookoutequipmentListLabelsOptions(
[property: CliOption("--label-group-name")] string LabelGroupName
) : AwsOptions
{
    [CliOption("--interval-start-time")]
    public long? IntervalStartTime { get; set; }

    [CliOption("--interval-end-time")]
    public long? IntervalEndTime { get; set; }

    [CliOption("--fault-code")]
    public string? FaultCode { get; set; }

    [CliOption("--equipment")]
    public string? Equipment { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}