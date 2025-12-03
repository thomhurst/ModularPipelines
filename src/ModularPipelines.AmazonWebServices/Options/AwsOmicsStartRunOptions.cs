using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("omics", "start-run")]
public record AwsOmicsStartRunOptions(
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--workflow-id")]
    public string? WorkflowId { get; set; }

    [CliOption("--workflow-type")]
    public string? WorkflowType { get; set; }

    [CliOption("--run-id")]
    public string? RunId { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--run-group-id")]
    public string? RunGroupId { get; set; }

    [CliOption("--priority")]
    public int? Priority { get; set; }

    [CliOption("--parameters")]
    public string? Parameters { get; set; }

    [CliOption("--storage-capacity")]
    public int? StorageCapacity { get; set; }

    [CliOption("--output-uri")]
    public string? OutputUri { get; set; }

    [CliOption("--log-level")]
    public string? LogLevel { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--request-id")]
    public string? RequestId { get; set; }

    [CliOption("--retention-mode")]
    public string? RetentionMode { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}