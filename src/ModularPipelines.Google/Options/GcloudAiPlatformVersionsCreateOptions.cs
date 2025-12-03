using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai-platform", "versions", "create")]
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

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string GcloudAiPlatformVersionsCreateOptionsVersion { get; set; }

    [CliOption("--model")]
    public string Model { get; set; }

    [CliOption("--accelerator")]
    public string[]? Accelerator { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--config")]
    public string? Config { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--framework")]
    public string? Framework { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--machine-type")]
    public string? MachineType { get; set; }

    [CliOption("--origin")]
    public string? Origin { get; set; }

    [CliOption("--python-version")]
    public string? PythonVersion { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--runtime-version")]
    public string? RuntimeVersion { get; set; }

    [CliOption("--staging-bucket")]
    public string? StagingBucket { get; set; }

    [CliOption("--max-nodes")]
    public string? MaxNodes { get; set; }

    [CliOption("--metric-targets")]
    public string[]? MetricTargets { get; set; }

    [CliOption("--min-nodes")]
    public string? MinNodes { get; set; }
}
