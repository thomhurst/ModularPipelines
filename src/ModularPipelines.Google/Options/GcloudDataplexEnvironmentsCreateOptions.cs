using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "environments", "create")]
public record GcloudDataplexEnvironmentsCreateOptions(
[property: CliArgument] string Environment,
[property: CliArgument] string Lake,
[property: CliArgument] string Location,
[property: CliOption("--os-image-version")] string OsImageVersion,
[property: CliOption("--os-image-java-libraries")] string[] OsImageJavaLibraries,
[property: CliOption("--os-image-properties")] string[] OsImageProperties,
[property: CliOption("--os-image-python-packages")] string[] OsImagePythonPackages,
[property: CliOption("--compute-disk-size-gb")] string ComputeDiskSizeGb,
[property: CliOption("--compute-max-node-count")] string ComputeMaxNodeCount,
[property: CliOption("--compute-node-count")] string ComputeNodeCount
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

    [CliFlag("--session-enable-fast-startup")]
    public bool? SessionEnableFastStartup { get; set; }

    [CliOption("--session-max-idle-duration")]
    public string? SessionMaxIdleDuration { get; set; }
}