using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("config", "unset")]
public record GcloudConfigUnsetOptions(
[property: PositionalArgument] string Section
) : GcloudOptions
{
    [BooleanCommandSwitch("--installation")]
    public bool? Installation { get; set; }
}