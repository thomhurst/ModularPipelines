using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("active-directory", "domains", "update")]
public record GcloudActiveDirectoryDomainsUpdateOptions(
[property: CliArgument] string Domain
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--enable-audit-logs")]
    public bool? EnableAuditLogs { get; set; }

    [CliOption("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CliOption("--add-authorized-networks")]
    public string[]? AddAuthorizedNetworks { get; set; }

    [CliOption("--remove-authorized-networks")]
    public string[]? RemoveAuthorizedNetworks { get; set; }

    [CliOption("--add-region")]
    public string? AddRegion { get; set; }

    [CliOption("--remove-region")]
    public string? RemoveRegion { get; set; }

    [CliFlag("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CliOption("--remove-labels")]
    public string[]? RemoveLabels { get; set; }
}