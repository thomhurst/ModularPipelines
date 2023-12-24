using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "create-job")]
public record AwsGlueCreateJobOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--role")] string Role,
[property: CommandSwitch("--command")] string Command
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--log-uri")]
    public string? LogUri { get; set; }

    [CommandSwitch("--execution-property")]
    public string? ExecutionProperty { get; set; }

    [CommandSwitch("--default-arguments")]
    public IEnumerable<KeyValue>? DefaultArguments { get; set; }

    [CommandSwitch("--non-overridable-arguments")]
    public IEnumerable<KeyValue>? NonOverridableArguments { get; set; }

    [CommandSwitch("--connections")]
    public string? Connections { get; set; }

    [CommandSwitch("--max-retries")]
    public int? MaxRetries { get; set; }

    [CommandSwitch("--allocated-capacity")]
    public int? AllocatedCapacity { get; set; }

    [CommandSwitch("--timeout")]
    public int? Timeout { get; set; }

    [CommandSwitch("--max-capacity")]
    public double? MaxCapacity { get; set; }

    [CommandSwitch("--security-configuration")]
    public string? SecurityConfiguration { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--notification-property")]
    public string? NotificationProperty { get; set; }

    [CommandSwitch("--glue-version")]
    public string? GlueVersion { get; set; }

    [CommandSwitch("--number-of-workers")]
    public int? NumberOfWorkers { get; set; }

    [CommandSwitch("--worker-type")]
    public string? WorkerType { get; set; }

    [CommandSwitch("--code-gen-configuration-nodes")]
    public IEnumerable<KeyValue>? CodeGenConfigurationNodes { get; set; }

    [CommandSwitch("--execution-class")]
    public string? ExecutionClass { get; set; }

    [CommandSwitch("--source-control-details")]
    public string? SourceControlDetails { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}