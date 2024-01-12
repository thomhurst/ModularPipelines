using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigee", "applications", "list")]
public record GcloudApigeeApplicationsListOptions : GcloudOptions
{
    [CommandSwitch("--developer")]
    public string? Developer { get; set; }

    [CommandSwitch("--organization")]
    public string? Organization { get; set; }
}