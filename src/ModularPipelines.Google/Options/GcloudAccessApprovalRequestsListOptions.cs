using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access-approval", "requests", "list")]
public record GcloudAccessApprovalRequestsListOptions : GcloudOptions
{
    [CommandSwitch("--state")]
    public string? State { get; set; }

    [CommandSwitch("--folder")]
    public string? Folder { get; set; }

    [CommandSwitch("--organization")]
    public string? Organization { get; set; }

    [CommandSwitch("--project")]
    public new string? Project { get; set; }
}