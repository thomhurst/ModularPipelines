using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("builds", "triggers", "list")]
public record GcloudBuildsTriggersListOptions : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}