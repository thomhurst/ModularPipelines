using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("builds", "connections", "list")]
public record GcloudBuildsConnectionsListOptions : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}