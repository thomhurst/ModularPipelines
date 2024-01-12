using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("config", "list")]
public record GcloudConfigListOptions(
[property: PositionalArgument] string Section
) : GcloudOptions
{
    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }
}