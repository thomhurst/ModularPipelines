using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataplex", "content", "create")]
public record GcloudDataplexContentCreateOptions(
[property: CommandSwitch("--data-text")] string DataText,
[property: CommandSwitch("--path")] string Path,
[property: CommandSwitch("--kernel-type")] string KernelType,
[property: BooleanCommandSwitch("PYTHON3")] bool PYTHON3,
[property: CommandSwitch("--query-engine")] string QueryEngine,
[property: BooleanCommandSwitch("SPARK")] bool Spark,
[property: CommandSwitch("--lake")] string Lake,
[property: CommandSwitch("--location")] string Location
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [BooleanCommandSwitch("--validate-only")]
    public bool? ValidateOnly { get; set; }
}