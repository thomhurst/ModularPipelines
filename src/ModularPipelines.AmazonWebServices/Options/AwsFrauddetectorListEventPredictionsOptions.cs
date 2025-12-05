using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("frauddetector", "list-event-predictions")]
public record AwsFrauddetectorListEventPredictionsOptions : AwsOptions
{
    [CliOption("--event-id")]
    public string? EventId { get; set; }

    [CliOption("--event-type")]
    public string? EventType { get; set; }

    [CliOption("--detector-id")]
    public string? DetectorId { get; set; }

    [CliOption("--detector-version-id")]
    public string? DetectorVersionId { get; set; }

    [CliOption("--prediction-time-range")]
    public string? PredictionTimeRange { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}