using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr-serverless", "create-application")]
public record AwsEmrServerlessCreateApplicationOptions(
[property: CommandSwitch("--release-label")] string ReleaseLabel,
[property: CommandSwitch("--type")] string Type
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--initial-capacity")]
    public IEnumerable<KeyValue>? InitialCapacity { get; set; }

    [CommandSwitch("--maximum-capacity")]
    public string? MaximumCapacity { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--auto-start-configuration")]
    public string? AutoStartConfiguration { get; set; }

    [CommandSwitch("--auto-stop-configuration")]
    public string? AutoStopConfiguration { get; set; }

    [CommandSwitch("--network-configuration")]
    public string? NetworkConfiguration { get; set; }

    [CommandSwitch("--architecture")]
    public string? Architecture { get; set; }

    [CommandSwitch("--image-configuration")]
    public string? ImageConfiguration { get; set; }

    [CommandSwitch("--worker-type-specifications")]
    public IEnumerable<KeyValue>? WorkerTypeSpecifications { get; set; }

    [CommandSwitch("--runtime-configuration")]
    public string[]? RuntimeConfiguration { get; set; }

    [CommandSwitch("--monitoring-configuration")]
    public string? MonitoringConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}