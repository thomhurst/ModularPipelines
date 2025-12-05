using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("stepfunctions", "update-map-run")]
public record AwsStepfunctionsUpdateMapRunOptions(
[property: CliOption("--map-run-arn")] string MapRunArn
) : AwsOptions
{
    [CliOption("--max-concurrency")]
    public int? MaxConcurrency { get; set; }

    [CliOption("--tolerated-failure-percentage")]
    public float? ToleratedFailurePercentage { get; set; }

    [CliOption("--tolerated-failure-count")]
    public long? ToleratedFailureCount { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}