using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lookoutequipment", "list-sensor-statistics")]
public record AwsLookoutequipmentListSensorStatisticsOptions(
[property: CommandSwitch("--dataset-name")] string DatasetName
) : AwsOptions
{
    [CommandSwitch("--ingestion-job-id")]
    public string? IngestionJobId { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}