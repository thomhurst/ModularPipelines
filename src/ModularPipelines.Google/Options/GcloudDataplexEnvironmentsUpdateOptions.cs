using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "environments", "update")]
public record GcloudDataplexEnvironmentsUpdateOptions(
[property: CliArgument] string Environment,
[property: CliArgument] string Lake,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CliOption("--compute-disk-size-gb")]
    public string? ComputeDiskSizeGb { get; set; }

    [CliOption("--compute-max-node-count")]
    public string? ComputeMaxNodeCount { get; set; }

    [CliOption("--compute-node-count")]
    public string? ComputeNodeCount { get; set; }

    [CliOption("--os-image-java-libraries")]
    public string[]? OsImageJavaLibraries { get; set; }

    [CliOption("--os-image-properties")]
    public string[]? OsImageProperties { get; set; }

    [CliOption("--os-image-python-packages")]
    public string[]? OsImagePythonPackages { get; set; }

    [CliOption("--os-image-version")]
    public string? OsImageVersion { get; set; }

    [CliFlag("--session-enable-fast-startup")]
    public bool? SessionEnableFastStartup { get; set; }

    [CliOption("--session-max-idle-duration")]
    public string? SessionMaxIdleDuration { get; set; }
}