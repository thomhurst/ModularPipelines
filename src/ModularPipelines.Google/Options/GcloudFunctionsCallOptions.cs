using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functions", "call")]
public record GcloudFunctionsCallOptions(
[property: PositionalArgument] string Name,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [BooleanCommandSwitch("--gen2")]
    public bool? Gen2 { get; set; }

    [CommandSwitch("--cloud-event")]
    public string? CloudEvent { get; set; }

    [CommandSwitch("--data")]
    public string? Data { get; set; }
}