using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("metastore", "federations", "update")]
public record GcloudMetastoreFederationsUpdateOptions(
[property: PositionalArgument] string Federation,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--update-backends")] string UpdateBackends,
[property: BooleanCommandSwitch("--clear-backends")] bool ClearBackends,
[property: CommandSwitch("--remove-backends")] string RemoveBackends
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [BooleanCommandSwitch("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CommandSwitch("--remove-labels")]
    public string[]? RemoveLabels { get; set; }
}