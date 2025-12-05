using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataflow", "jobs", "run")]
public record GcloudDataflowJobsRunOptions(
[property: CliArgument] string JobName,
[property: CliOption("--gcs-location")] string GcsLocation
) : GcloudOptions
{
    [CliOption("--additional-experiments")]
    public string[]? AdditionalExperiments { get; set; }

    [CliOption("--dataflow-kms-key")]
    public string? DataflowKmsKey { get; set; }

    [CliFlag("--disable-public-ips")]
    public bool? DisablePublicIps { get; set; }

    [CliFlag("--enable-streaming-engine")]
    public bool? EnableStreamingEngine { get; set; }

    [CliOption("--max-workers")]
    public string? MaxWorkers { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--num-workers")]
    public string? NumWorkers { get; set; }

    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--service-account-email")]
    public string? ServiceAccountEmail { get; set; }

    [CliOption("--staging-location")]
    public string? StagingLocation { get; set; }

    [CliOption("--subnetwork")]
    public string? Subnetwork { get; set; }

    [CliOption("--worker-machine-type")]
    public string? WorkerMachineType { get; set; }

    [CliOption("--worker-region")]
    public string? WorkerRegion { get; set; }

    [CliOption("--worker-zone")]
    public string? WorkerZone { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}