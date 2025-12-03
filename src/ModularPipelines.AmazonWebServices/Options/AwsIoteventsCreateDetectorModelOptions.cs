using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iotevents", "create-detector-model")]
public record AwsIoteventsCreateDetectorModelOptions(
[property: CliOption("--detector-model-name")] string DetectorModelName,
[property: CliOption("--detector-model-definition")] string DetectorModelDefinition,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--detector-model-description")]
    public string? DetectorModelDescription { get; set; }

    [CliOption("--key")]
    public string? Key { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--evaluation-method")]
    public string? EvaluationMethod { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}