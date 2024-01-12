using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataflow", "flex-template", "run")]
public record GcloudDataflowFlexTemplateRunOptions(
[property: PositionalArgument] string JobName,
[property: CommandSwitch("--template-file-gcs-location")] string TemplateFileGcsLocation
) : GcloudOptions
{
    [CommandSwitch("--additional-experiments")]
    public string[]? AdditionalExperiments { get; set; }

    [CommandSwitch("--additional-user-labels")]
    public string[]? AdditionalUserLabels { get; set; }

    [CommandSwitch("--dataflow-kms-key")]
    public string? DataflowKmsKey { get; set; }

    [BooleanCommandSwitch("--disable-public-ips")]
    public bool? DisablePublicIps { get; set; }

    [BooleanCommandSwitch("--enable-streaming-engine")]
    public bool? EnableStreamingEngine { get; set; }

    [CommandSwitch("--flexrs-goal")]
    public string? FlexrsGoal { get; set; }

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

    [CommandSwitch("--temp-location")]
    public string? TempLocation { get; set; }

    [CommandSwitch("--worker-machine-type")]
    public string? WorkerMachineType { get; set; }

    [CommandSwitch("--[no-]update")]
    public string[]? NoUpdate { get; set; }

    [CommandSwitch("--transform-name-mappings")]
    public string[]? TransformNameMappings { get; set; }

    [CommandSwitch("--worker-region")]
    public string? WorkerRegion { get; set; }

    [CommandSwitch("--worker-zone")]
    public string? WorkerZone { get; set; }
}