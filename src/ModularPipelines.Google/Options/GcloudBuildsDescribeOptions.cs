using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("builds", "describe")]
public record GcloudBuildsDescribeOptions(
[property: PositionalArgument] string Build
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}