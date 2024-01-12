using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access-context-manager", "perimeters", "dry-run", "list")]
public record GcloudAccessContextManagerPerimetersDryRunListOptions : GcloudOptions
{
    [CommandSwitch("--policy")]
    public string? Policy { get; set; }
}