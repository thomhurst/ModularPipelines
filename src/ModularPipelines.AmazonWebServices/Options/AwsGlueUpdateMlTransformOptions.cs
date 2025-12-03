using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "update-ml-transform")]
public record AwsGlueUpdateMlTransformOptions(
[property: CliOption("--transform-id")] string TransformId
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--parameters")]
    public string? Parameters { get; set; }

    [CliOption("--role")]
    public string? Role { get; set; }

    [CliOption("--glue-version")]
    public string? GlueVersion { get; set; }

    [CliOption("--max-capacity")]
    public double? MaxCapacity { get; set; }

    [CliOption("--worker-type")]
    public string? WorkerType { get; set; }

    [CliOption("--number-of-workers")]
    public int? NumberOfWorkers { get; set; }

    [CliOption("--timeout")]
    public int? Timeout { get; set; }

    [CliOption("--max-retries")]
    public int? MaxRetries { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}