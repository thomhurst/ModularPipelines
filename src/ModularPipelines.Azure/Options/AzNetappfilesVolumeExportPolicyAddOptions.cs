using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("netappfiles", "volume", "export-policy", "add")]
public record AzNetappfilesVolumeExportPolicyAddOptions(
[property: CliFlag("--allowed-clients")] bool AllowedClients,
[property: CliFlag("--cifs")] bool Cifs,
[property: CliFlag("--nfsv3")] bool Nfsv3,
[property: CliFlag("--nfsv41")] bool Nfsv41,
[property: CliFlag("--unix-read-only")] bool UnixReadOnly,
[property: CliFlag("--unix-read-write")] bool UnixReadWrite
) : AzOptions
{
    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--chown-mode")]
    public string? ChownMode { get; set; }

    [CliFlag("--has-root-access")]
    public bool? HasRootAccess { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--kerberos5-r")]
    public string? Kerberos5R { get; set; }

    [CliOption("--kerberos5-rw")]
    public string? Kerberos5Rw { get; set; }

    [CliOption("--kerberos5i-r")]
    public string? Kerberos5iR { get; set; }

    [CliOption("--kerberos5i-rw")]
    public string? Kerberos5iRw { get; set; }

    [CliOption("--kerberos5p-r")]
    public string? Kerberos5pR { get; set; }

    [CliOption("--kerberos5p-rw")]
    public string? Kerberos5pRw { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--pool-name")]
    public string? PoolName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--rule-index")]
    public string? RuleIndex { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}