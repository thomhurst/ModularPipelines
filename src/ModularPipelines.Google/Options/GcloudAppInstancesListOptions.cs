using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("app", "instances", "list")]
public record GcloudAppInstancesListOptions : GcloudOptions
{
    [CommandSwitch("--service")]
    public string? Service { get; set; }

    [CommandSwitch("--version")]
    public new string? Version { get; set; }
}