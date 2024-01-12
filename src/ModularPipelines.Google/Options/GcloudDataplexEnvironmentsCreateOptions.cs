using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataplex", "environments", "create")]
public record GcloudDataplexEnvironmentsCreateOptions(
[property: PositionalArgument] string Environment,
[property: PositionalArgument] string Lake,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--os-image-version")] string OsImageVersion,
[property: CommandSwitch("--os-image-java-libraries")] string[] OsImageJavaLibraries,
[property: CommandSwitch("--os-image-properties")] string[] OsImageProperties,
[property: CommandSwitch("--os-image-python-packages")] string[] OsImagePythonPackages,
[property: CommandSwitch("--compute-disk-size-gb")] string ComputeDiskSizeGb,
[property: CommandSwitch("--compute-max-node-count")] string ComputeMaxNodeCount,
[property: CommandSwitch("--compute-node-count")] string ComputeNodeCount
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

    [BooleanCommandSwitch("--session-enable-fast-startup")]
    public bool? SessionEnableFastStartup { get; set; }

    [CommandSwitch("--session-max-idle-duration")]
    public string? SessionMaxIdleDuration { get; set; }
}