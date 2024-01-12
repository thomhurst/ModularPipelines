using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("components", "update")]
public record GcloudComponentsUpdateOptions : GcloudOptions
{
    [CommandSwitch("--version")]
    public new string? Version { get; set; }
}