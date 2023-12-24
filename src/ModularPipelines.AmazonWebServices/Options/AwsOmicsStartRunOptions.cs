using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("omics", "start-run")]
public record AwsOmicsStartRunOptions(
[property: CommandSwitch("--role-arn")] string RoleArn
) : AwsOptions
{
    [CommandSwitch("--workflow-id")]
    public string? WorkflowId { get; set; }

    [CommandSwitch("--workflow-type")]
    public string? WorkflowType { get; set; }

    [CommandSwitch("--run-id")]
    public string? RunId { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--run-group-id")]
    public string? RunGroupId { get; set; }

    [CommandSwitch("--priority")]
    public int? Priority { get; set; }

    [CommandSwitch("--parameters")]
    public string? Parameters { get; set; }

    [CommandSwitch("--storage-capacity")]
    public int? StorageCapacity { get; set; }

    [CommandSwitch("--output-uri")]
    public string? OutputUri { get; set; }

    [CommandSwitch("--log-level")]
    public string? LogLevel { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--request-id")]
    public string? RequestId { get; set; }

    [CommandSwitch("--retention-mode")]
    public string? RetentionMode { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}