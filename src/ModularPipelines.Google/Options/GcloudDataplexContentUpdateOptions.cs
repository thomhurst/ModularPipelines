using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataplex", "content", "update")]
public record GcloudDataplexContentUpdateOptions(
[property: PositionalArgument] string Content,
[property: PositionalArgument] string Lake,
[property: PositionalArgument] string Location
) : GcloudOptions
{
    [CommandSwitch("--data-text")]
    public string? DataText { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [BooleanCommandSwitch("--validate-only")]
    public bool? ValidateOnly { get; set; }

    [CommandSwitch("--kernel-type")]
    public string? KernelType { get; set; }

    [BooleanCommandSwitch("PYTHON3")]
    public bool? PYTHON3 { get; set; }

    [CommandSwitch("--query-engine")]
    public string? QueryEngine { get; set; }

    [BooleanCommandSwitch("SPARK")]
    public bool? Spark { get; set; }
}