using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutequipment", "list-inference-executions")]
public record AwsLookoutequipmentListInferenceExecutionsOptions(
[property: CliOption("--inference-scheduler-name")] string InferenceSchedulerName
) : AwsOptions
{
    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--data-start-time-after")]
    public long? DataStartTimeAfter { get; set; }

    [CliOption("--data-end-time-before")]
    public long? DataEndTimeBefore { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}