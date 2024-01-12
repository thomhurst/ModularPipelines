using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("active-directory", "domains", "create")]
public record GcloudActiveDirectoryDomainsCreateOptions(
[property: PositionalArgument] string Domain,
[property: CommandSwitch("--region")] string[] Region,
[property: CommandSwitch("--reserved-ip-range")] string ReservedIpRange
) : GcloudOptions
{
    [CommandSwitch("--admin-name")]
    public string? AdminName { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--authorized-networks")]
    public string[]? AuthorizedNetworks { get; set; }

    [BooleanCommandSwitch("--enable-audit-logs")]
    public bool? EnableAuditLogs { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}