using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataplex", "environments", "update")]
public record GcloudDataplexEnvironmentsUpdateOptions(
[property: PositionalArgument] string Environment,
[property: PositionalArgument] string Lake,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [BooleanCommandSwitch("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CommandSwitch("--compute-disk-size-gb")]
    public string? ComputeDiskSizeGb { get; set; }

    [CommandSwitch("--compute-max-node-count")]
    public string? ComputeMaxNodeCount { get; set; }

    [CommandSwitch("--compute-node-count")]
    public string? ComputeNodeCount { get; set; }

    [CommandSwitch("--os-image-java-libraries")]
    public string[]? OsImageJavaLibraries { get; set; }

    [CommandSwitch("--os-image-properties")]
    public string[]? OsImageProperties { get; set; }

    [CommandSwitch("--os-image-python-packages")]
    public string[]? OsImagePythonPackages { get; set; }

    [CommandSwitch("--os-image-version")]
    public string? OsImageVersion { get; set; }

    [BooleanCommandSwitch("--session-enable-fast-startup")]
    public bool? SessionEnableFastStartup { get; set; }

    [CommandSwitch("--session-max-idle-duration")]
    public string? SessionMaxIdleDuration { get; set; }
}