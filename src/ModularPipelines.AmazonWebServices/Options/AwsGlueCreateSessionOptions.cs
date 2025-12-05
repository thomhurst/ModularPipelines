using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "create-session")]
public record AwsGlueCreateSessionOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--role")] string Role,
[property: CliOption("--command")] string Command
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--timeout")]
    public int? Timeout { get; set; }

    [CliOption("--idle-timeout")]
    public int? IdleTimeout { get; set; }

    [CliOption("--default-arguments")]
    public IEnumerable<KeyValue>? DefaultArguments { get; set; }

    [CliOption("--connections")]
    public string? Connections { get; set; }

    [CliOption("--max-capacity")]
    public double? MaxCapacity { get; set; }

    [CliOption("--number-of-workers")]
    public int? NumberOfWorkers { get; set; }

    [CliOption("--worker-type")]
    public string? WorkerType { get; set; }

    [CliOption("--security-configuration")]
    public string? SecurityConfiguration { get; set; }

    [CliOption("--glue-version")]
    public string? GlueVersion { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--request-origin")]
    public string? RequestOrigin { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}