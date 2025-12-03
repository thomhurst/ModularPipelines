using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("bms", "nfs-shares", "update")]
public record GcloudBmsNfsSharesUpdateOptions(
[property: CliArgument] string NfsShare,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CliOption("--add-allowed-client")]
    public string[]? AddAllowedClient { get; set; }

    [CliFlag("network")]
    public bool? Network { get; set; }

    [CliFlag("network-project-id")]
    public bool? NetworkProjectId { get; set; }

    [CliFlag("cidr")]
    public bool? Cidr { get; set; }

    [CliFlag("mount-permissions")]
    public bool? MountPermissions { get; set; }

    [CliFlag("allow-dev")]
    public bool? AllowDev { get; set; }

    [CliFlag("allow-suid")]
    public bool? AllowSuid { get; set; }

    [CliFlag("enable-root-squash")]
    public bool? EnableRootSquash { get; set; }

    [CliFlag("--clear-allowed-clients")]
    public bool? ClearAllowedClients { get; set; }

    [CliOption("--remove-allowed-client")]
    public string[]? RemoveAllowedClient { get; set; }

    [CliFlag("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CliOption("--remove-labels")]
    public string[]? RemoveLabels { get; set; }
}