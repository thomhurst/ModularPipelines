using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataflow", "jobs", "run")]
public record GcloudDataflowJobsRunOptions(
[property: PositionalArgument] string JobName,
[property: CommandSwitch("--gcs-location")] string GcsLocation
) : GcloudOptions
{
    [CommandSwitch("--additional-experiments")]
    public string[]? AdditionalExperiments { get; set; }

    [CommandSwitch("--dataflow-kms-key")]
    public string? DataflowKmsKey { get; set; }

    [BooleanCommandSwitch("--disable-public-ips")]
    public bool? DisablePublicIps { get; set; }

    [BooleanCommandSwitch("--enable-streaming-engine")]
    public bool? EnableStreamingEngine { get; set; }

    [CommandSwitch("--max-workers")]
    public string? MaxWorkers { get; set; }

    [CommandSwitch("--network")]
    public string? Network { get; set; }

    [CommandSwitch("--num-workers")]
    public string? NumWorkers { get; set; }

    [CommandSwitch("--parameters")]
    public string[]? Parameters { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--service-account-email")]
    public string? ServiceAccountEmail { get; set; }

    [CommandSwitch("--staging-location")]
    public string? StagingLocation { get; set; }

    [CommandSwitch("--subnetwork")]
    public string? Subnetwork { get; set; }

    [CommandSwitch("--worker-machine-type")]
    public string? WorkerMachineType { get; set; }

    [CommandSwitch("--worker-region")]
    public string? WorkerRegion { get; set; }

    [CommandSwitch("--worker-zone")]
    public string? WorkerZone { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}