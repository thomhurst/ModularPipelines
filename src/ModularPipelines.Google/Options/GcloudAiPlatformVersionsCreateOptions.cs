using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai-platform", "versions", "create")]
public record GcloudAiPlatformVersionsCreateOptions : GcloudOptions
{
    public GcloudAiPlatformVersionsCreateOptions(
        string version,
        string model
    )
    {
        GcloudAiPlatformVersionsCreateOptionsVersion = version;
        Model = model;
    }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string GcloudAiPlatformVersionsCreateOptionsVersion { get; set; }

    [CommandSwitch("--model")]
    public string Model { get; set; }

    [CommandSwitch("--accelerator")]
    public string[]? Accelerator { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--config")]
    public string? Config { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--framework")]
    public string? Framework { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--machine-type")]
    public string? MachineType { get; set; }

    [CommandSwitch("--origin")]
    public string? Origin { get; set; }

    [CommandSwitch("--python-version")]
    public string? PythonVersion { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--runtime-version")]
    public string? RuntimeVersion { get; set; }

    [CommandSwitch("--staging-bucket")]
    public string? StagingBucket { get; set; }

    [CommandSwitch("--max-nodes")]
    public string? MaxNodes { get; set; }

    [CommandSwitch("--metric-targets")]
    public string[]? MetricTargets { get; set; }

    [CommandSwitch("--min-nodes")]
    public string? MinNodes { get; set; }
}
