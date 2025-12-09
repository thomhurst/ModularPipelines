using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("frauddetector", "create-detector-version")]
public record AwsFrauddetectorCreateDetectorVersionOptions(
[property: CliOption("--detector-id")] string DetectorId,
[property: CliOption("--rules")] string[] Rules
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--external-model-endpoints")]
    public string[]? ExternalModelEndpoints { get; set; }

    [CliOption("--model-versions")]
    public string[]? ModelVersions { get; set; }

    [CliOption("--rule-execution-mode")]
    public string? RuleExecutionMode { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}