using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr-serverless", "update-application")]
public record AwsEmrServerlessUpdateApplicationOptions(
[property: CommandSwitch("--application-id")] string ApplicationId
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--initial-capacity")]
    public IEnumerable<KeyValue>? InitialCapacity { get; set; }

    [CommandSwitch("--maximum-capacity")]
    public string? MaximumCapacity { get; set; }

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

    [CommandSwitch("--release-label")]
    public string? ReleaseLabel { get; set; }

    [CommandSwitch("--runtime-configuration")]
    public string[]? RuntimeConfiguration { get; set; }

    [CommandSwitch("--monitoring-configuration")]
    public string? MonitoringConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}