using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigee", "deployments", "list")]
public record GcloudApigeeDeploymentsListOptions : GcloudOptions
{
    [CommandSwitch("--api")]
    public string? Api { get; set; }

    [CommandSwitch("--environment")]
    public string? Environment { get; set; }

    [CommandSwitch("--organization")]
    public string? Organization { get; set; }

    [CommandSwitch("--revision")]
    public string? Revision { get; set; }
}