using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access-context-manager", "perimeters", "list")]
public record GcloudAccessContextManagerPerimetersListOptions : GcloudOptions
{
    [CommandSwitch("--policy")]
    public string? Policy { get; set; }
}