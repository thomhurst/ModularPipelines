using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "create-job")]
public record AwsGlueCreateJobOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--role")] string Role,
[property: CliOption("--command")] string Command
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--log-uri")]
    public string? LogUri { get; set; }

    [CliOption("--execution-property")]
    public string? ExecutionProperty { get; set; }

    [CliOption("--default-arguments")]
    public IEnumerable<KeyValue>? DefaultArguments { get; set; }

    [CliOption("--non-overridable-arguments")]
    public IEnumerable<KeyValue>? NonOverridableArguments { get; set; }

    [CliOption("--connections")]
    public string? Connections { get; set; }

    [CliOption("--max-retries")]
    public int? MaxRetries { get; set; }

    [CliOption("--allocated-capacity")]
    public int? AllocatedCapacity { get; set; }

    [CliOption("--timeout")]
    public int? Timeout { get; set; }

    [CliOption("--max-capacity")]
    public double? MaxCapacity { get; set; }

    [CliOption("--security-configuration")]
    public string? SecurityConfiguration { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--notification-property")]
    public string? NotificationProperty { get; set; }

    [CliOption("--glue-version")]
    public string? GlueVersion { get; set; }

    [CliOption("--number-of-workers")]
    public int? NumberOfWorkers { get; set; }

    [CliOption("--worker-type")]
    public string? WorkerType { get; set; }

    [CliOption("--code-gen-configuration-nodes")]
    public IEnumerable<KeyValue>? CodeGenConfigurationNodes { get; set; }

    [CliOption("--execution-class")]
    public string? ExecutionClass { get; set; }

    [CliOption("--source-control-details")]
    public string? SourceControlDetails { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}