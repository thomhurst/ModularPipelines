using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("bms", "nfs-shares", "update")]
public record GcloudBmsNfsSharesUpdateOptions(
[property: PositionalArgument] string NfsShare,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CommandSwitch("--add-allowed-client")]
    public string[]? AddAllowedClient { get; set; }

    [BooleanCommandSwitch("network")]
    public bool? Network { get; set; }

    [BooleanCommandSwitch("network-project-id")]
    public bool? NetworkProjectId { get; set; }

    [BooleanCommandSwitch("cidr")]
    public bool? Cidr { get; set; }

    [BooleanCommandSwitch("mount-permissions")]
    public bool? MountPermissions { get; set; }

    [BooleanCommandSwitch("allow-dev")]
    public bool? AllowDev { get; set; }

    [BooleanCommandSwitch("allow-suid")]
    public bool? AllowSuid { get; set; }

    [BooleanCommandSwitch("enable-root-squash")]
    public bool? EnableRootSquash { get; set; }

    [BooleanCommandSwitch("--clear-allowed-clients")]
    public bool? ClearAllowedClients { get; set; }

    [CommandSwitch("--remove-allowed-client")]
    public string[]? RemoveAllowedClient { get; set; }

    [BooleanCommandSwitch("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CommandSwitch("--remove-labels")]
    public string[]? RemoveLabels { get; set; }
}