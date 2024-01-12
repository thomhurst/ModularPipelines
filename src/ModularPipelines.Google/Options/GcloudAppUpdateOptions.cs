using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("app", "update")]
public record GcloudAppUpdateOptions : GcloudOptions
{
    [CommandSwitch("--service-account")]
    public string? ServiceAccount { get; set; }

    [CommandSwitch("--[no-]split-health-checks")]
    public string[]? NoSplitHealthChecks { get; set; }
}