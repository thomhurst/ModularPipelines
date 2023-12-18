using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netappfiles", "volume", "export-policy", "add")]
public record AzNetappfilesVolumeExportPolicyAddOptions(
[property: BooleanCommandSwitch("--allowed-clients")] bool AllowedClients,
[property: BooleanCommandSwitch("--cifs")] bool Cifs,
[property: BooleanCommandSwitch("--nfsv3")] bool Nfsv3,
[property: BooleanCommandSwitch("--nfsv41")] bool Nfsv41,
[property: BooleanCommandSwitch("--unix-read-only")] bool UnixReadOnly,
[property: BooleanCommandSwitch("--unix-read-write")] bool UnixReadWrite
) : AzOptions
{
    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--chown-mode")]
    public string? ChownMode { get; set; }

    [BooleanCommandSwitch("--has-root-access")]
    public bool? HasRootAccess { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--kerberos5-r")]
    public string? Kerberos5R { get; set; }

    [CommandSwitch("--kerberos5-rw")]
    public string? Kerberos5Rw { get; set; }

    [CommandSwitch("--kerberos5i-r")]
    public string? Kerberos5iR { get; set; }

    [CommandSwitch("--kerberos5i-rw")]
    public string? Kerberos5iRw { get; set; }

    [CommandSwitch("--kerberos5p-r")]
    public string? Kerberos5pR { get; set; }

    [CommandSwitch("--kerberos5p-rw")]
    public string? Kerberos5pRw { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--pool-name")]
    public string? PoolName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--rule-index")]
    public string? RuleIndex { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}

