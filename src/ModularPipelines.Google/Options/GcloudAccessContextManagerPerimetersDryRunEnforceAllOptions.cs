using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access-context-manager", "perimeters", "dry-run", "enforce-all")]
public record GcloudAccessContextManagerPerimetersDryRunEnforceAllOptions : GcloudOptions
{
    [CommandSwitch("--etag")]
    public string? Etag { get; set; }

    [CommandSwitch("--policy")]
    public string? Policy { get; set; }
}