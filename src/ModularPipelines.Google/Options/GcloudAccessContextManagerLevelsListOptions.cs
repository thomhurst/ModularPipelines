using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access-context-manager", "levels", "list")]
public record GcloudAccessContextManagerLevelsListOptions : GcloudOptions
{
    [CommandSwitch("--policy")]
    public string? Policy { get; set; }
}