using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functions", "delete")]
public record GcloudFunctionsDeleteOptions(
[property: PositionalArgument] string Name,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [BooleanCommandSwitch("--gen2")]
    public bool? Gen2 { get; set; }
}