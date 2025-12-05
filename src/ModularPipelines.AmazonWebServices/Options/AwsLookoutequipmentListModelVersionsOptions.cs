using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lookoutequipment", "list-model-versions")]
public record AwsLookoutequipmentListModelVersionsOptions(
[property: CliOption("--model-name")] string ModelName
) : AwsOptions
{
    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--source-type")]
    public string? SourceType { get; set; }

    [CliOption("--created-at-end-time")]
    public long? CreatedAtEndTime { get; set; }

    [CliOption("--created-at-start-time")]
    public long? CreatedAtStartTime { get; set; }

    [CliOption("--max-model-version")]
    public long? MaxModelVersion { get; set; }

    [CliOption("--min-model-version")]
    public long? MinModelVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}