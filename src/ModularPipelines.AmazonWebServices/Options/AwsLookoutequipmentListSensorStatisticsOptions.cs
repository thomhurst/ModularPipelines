using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutequipment", "list-sensor-statistics")]
public record AwsLookoutequipmentListSensorStatisticsOptions(
[property: CliOption("--dataset-name")] string DatasetName
) : AwsOptions
{
    [CliOption("--ingestion-job-id")]
    public string? IngestionJobId { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}