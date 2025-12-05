using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr-serverless", "create-application")]
public record AwsEmrServerlessCreateApplicationOptions(
[property: CliOption("--release-label")] string ReleaseLabel,
[property: CliOption("--type")] string Type
) : AwsOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--initial-capacity")]
    public IEnumerable<KeyValue>? InitialCapacity { get; set; }

    [CliOption("--maximum-capacity")]
    public string? MaximumCapacity { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--auto-start-configuration")]
    public string? AutoStartConfiguration { get; set; }

    [CliOption("--auto-stop-configuration")]
    public string? AutoStopConfiguration { get; set; }

    [CliOption("--network-configuration")]
    public string? NetworkConfiguration { get; set; }

    [CliOption("--architecture")]
    public string? Architecture { get; set; }

    [CliOption("--image-configuration")]
    public string? ImageConfiguration { get; set; }

    [CliOption("--worker-type-specifications")]
    public IEnumerable<KeyValue>? WorkerTypeSpecifications { get; set; }

    [CliOption("--runtime-configuration")]
    public string[]? RuntimeConfiguration { get; set; }

    [CliOption("--monitoring-configuration")]
    public string? MonitoringConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}