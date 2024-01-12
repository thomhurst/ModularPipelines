using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("metastore", "federations", "create")]
public record GcloudMetastoreFederationsCreateOptions(
[property: PositionalArgument] string Federation,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--backends")] string Backends
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--hive-metastore-version")]
    public string? HiveMetastoreVersion { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}