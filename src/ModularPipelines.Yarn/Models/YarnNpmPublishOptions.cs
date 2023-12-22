using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Yarn.Models;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("npm", "publish")]
public record YarnNpmPublishOptions : YarnOptions
{
    [CommandSwitch("--access")]
    public string? Access { get; set; }

    [CommandSwitch("--tag")]
    public string? Tag { get; set; }

    [BooleanCommandSwitch("--tolerate-republish")]
    public bool? TolerateRepublish { get; set; }

    [CommandSwitch("--otp")]
    public string? Otp { get; set; }
}