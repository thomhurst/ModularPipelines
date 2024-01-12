using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("active-directory", "domains", "update")]
public record GcloudActiveDirectoryDomainsUpdateOptions(
[property: PositionalArgument] string Domain
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [BooleanCommandSwitch("--enable-audit-logs")]
    public bool? EnableAuditLogs { get; set; }

    [CommandSwitch("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CommandSwitch("--add-authorized-networks")]
    public string[]? AddAuthorizedNetworks { get; set; }

    [CommandSwitch("--remove-authorized-networks")]
    public string[]? RemoveAuthorizedNetworks { get; set; }

    [CommandSwitch("--add-region")]
    public string? AddRegion { get; set; }

    [CommandSwitch("--remove-region")]
    public string? RemoveRegion { get; set; }

    [BooleanCommandSwitch("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CommandSwitch("--remove-labels")]
    public string[]? RemoveLabels { get; set; }
}