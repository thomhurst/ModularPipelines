using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "create-ml-transform")]
public record AwsGlueCreateMlTransformOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--input-record-tables")] string[] InputRecordTables,
[property: CommandSwitch("--parameters")] string Parameters,
[property: CommandSwitch("--role")] string Role
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--glue-version")]
    public string? GlueVersion { get; set; }

    [CommandSwitch("--max-capacity")]
    public double? MaxCapacity { get; set; }

    [CommandSwitch("--worker-type")]
    public string? WorkerType { get; set; }

    [CommandSwitch("--number-of-workers")]
    public int? NumberOfWorkers { get; set; }

    [CommandSwitch("--timeout")]
    public int? Timeout { get; set; }

    [CommandSwitch("--max-retries")]
    public int? MaxRetries { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--transform-encryption")]
    public string? TransformEncryption { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}