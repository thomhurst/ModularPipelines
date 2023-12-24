using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("frauddetector", "list-event-predictions")]
public record AwsFrauddetectorListEventPredictionsOptions : AwsOptions
{
    [CommandSwitch("--event-id")]
    public string? EventId { get; set; }

    [CommandSwitch("--event-type")]
    public string? EventType { get; set; }

    [CommandSwitch("--detector-id")]
    public string? DetectorId { get; set; }

    [CommandSwitch("--detector-version-id")]
    public string? DetectorVersionId { get; set; }

    [CommandSwitch("--prediction-time-range")]
    public string? PredictionTimeRange { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}