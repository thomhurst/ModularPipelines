using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr-serverless", "update-application")]
public record AwsEmrServerlessUpdateApplicationOptions(
[property: CliOption("--application-id")] string ApplicationId
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--initial-capacity")]
    public IEnumerable<KeyValue>? InitialCapacity { get; set; }

    [CliOption("--maximum-capacity")]
    public string? MaximumCapacity { get; set; }

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

    [CliOption("--release-label")]
    public string? ReleaseLabel { get; set; }

    [CliOption("--runtime-configuration")]
    public string[]? RuntimeConfiguration { get; set; }

    [CliOption("--monitoring-configuration")]
    public string? MonitoringConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}