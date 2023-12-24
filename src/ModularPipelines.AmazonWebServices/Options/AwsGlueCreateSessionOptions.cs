using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "create-session")]
public record AwsGlueCreateSessionOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--role")] string Role,
[property: CommandSwitch("--command")] string Command
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--timeout")]
    public int? Timeout { get; set; }

    [CommandSwitch("--idle-timeout")]
    public int? IdleTimeout { get; set; }

    [CommandSwitch("--default-arguments")]
    public IEnumerable<KeyValue>? DefaultArguments { get; set; }

    [CommandSwitch("--connections")]
    public string? Connections { get; set; }

    [CommandSwitch("--max-capacity")]
    public double? MaxCapacity { get; set; }

    [CommandSwitch("--number-of-workers")]
    public int? NumberOfWorkers { get; set; }

    [CommandSwitch("--worker-type")]
    public string? WorkerType { get; set; }

    [CommandSwitch("--security-configuration")]
    public string? SecurityConfiguration { get; set; }

    [CommandSwitch("--glue-version")]
    public string? GlueVersion { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--request-origin")]
    public string? RequestOrigin { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}