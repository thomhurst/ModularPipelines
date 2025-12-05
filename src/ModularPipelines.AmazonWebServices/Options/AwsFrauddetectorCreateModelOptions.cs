using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("frauddetector", "create-model")]
public record AwsFrauddetectorCreateModelOptions(
[property: CliOption("--model-id")] string ModelId,
[property: CliOption("--model-type")] string ModelType,
[property: CliOption("--event-type-name")] string EventTypeName
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}