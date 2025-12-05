using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("active-directory", "domains", "create")]
public record GcloudActiveDirectoryDomainsCreateOptions(
[property: CliArgument] string Domain,
[property: CliOption("--region")] string[] Region,
[property: CliOption("--reserved-ip-range")] string ReservedIpRange
) : GcloudOptions
{
    [CliOption("--admin-name")]
    public string? AdminName { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--authorized-networks")]
    public string[]? AuthorizedNetworks { get; set; }

    [CliFlag("--enable-audit-logs")]
    public bool? EnableAuditLogs { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}