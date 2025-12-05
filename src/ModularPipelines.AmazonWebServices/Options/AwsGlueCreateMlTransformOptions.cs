using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "create-ml-transform")]
public record AwsGlueCreateMlTransformOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--input-record-tables")] string[] InputRecordTables,
[property: CliOption("--parameters")] string Parameters,
[property: CliOption("--role")] string Role
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

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

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--transform-encryption")]
    public string? TransformEncryption { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}