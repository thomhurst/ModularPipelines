using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("config", "set")]
public record GcloudConfigSetOptions(
[property: PositionalArgument] string Section,
[property: PositionalArgument] string Value
) : GcloudOptions
{
    [BooleanCommandSwitch("--installation")]
    public bool? Installation { get; set; }
}