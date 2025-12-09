using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("frauddetector", "update-detector-version")]
public record AwsFrauddetectorUpdateDetectorVersionOptions(
[property: CliOption("--detector-id")] string DetectorId,
[property: CliOption("--detector-version-id")] string DetectorVersionId,
[property: CliOption("--external-model-endpoints")] string[] ExternalModelEndpoints,
[property: CliOption("--rules")] string[] Rules
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--model-versions")]
    public string[]? ModelVersions { get; set; }

    [CliOption("--rule-execution-mode")]
    public string? RuleExecutionMode { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}