using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("frauddetector", "create-model-version")]
public record AwsFrauddetectorCreateModelVersionOptions(
[property: CliOption("--model-id")] string ModelId,
[property: CliOption("--model-type")] string ModelType,
[property: CliOption("--training-data-source")] string TrainingDataSource,
[property: CliOption("--training-data-schema")] string TrainingDataSchema
) : AwsOptions
{
    [CliOption("--external-events-detail")]
    public string? ExternalEventsDetail { get; set; }

    [CliOption("--ingested-events-detail")]
    public string? IngestedEventsDetail { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}