using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("netappfiles", "account", "ad", "add")]
public record AzNetappfilesAccountAdAddOptions(
[property: CliOption("--dns")] string Dns,
[property: CliOption("--domain")] string Domain,
[property: CliOption("--password")] string Password,
[property: CliOption("--smb-server-name")] string SmbServerName,
[property: CliOption("--username")] string Username
) : AzOptions
{
    [CliOption("--account-name")]
    public int? AccountName { get; set; }

    [CliOption("--ad-name")]
    public string? AdName { get; set; }

    [CliOption("--administrators")]
    public string? Administrators { get; set; }

    [CliFlag("--aes-encryption")]
    public bool? AesEncryption { get; set; }

    [CliFlag("--allow-local-ldap-users")]
    public bool? AllowLocalLdapUsers { get; set; }

    [CliOption("--backup-operators")]
    public string? BackupOperators { get; set; }

    [CliFlag("--encrypt-dc-conn")]
    public bool? EncryptDcConn { get; set; }

    [CliOption("--group-dn")]
    public string? GroupDn { get; set; }

    [CliOption("--group-filter")]
    public string? GroupFilter { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--kdc-ip")]
    public string? KdcIp { get; set; }

    [CliOption("--ldap-over-tls")]
    public string? LdapOverTls { get; set; }

    [CliFlag("--ldap-signing")]
    public bool? LdapSigning { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--organizational-unit")]
    public string? OrganizationalUnit { get; set; }

    [CliOption("--preferred-servers-for-ldap-client")]
    public string? PreferredServersForLdapClient { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--security-operators")]
    public string? SecurityOperators { get; set; }

    [CliOption("--server-root-ca-cert")]
    public string? ServerRootCaCert { get; set; }

    [CliOption("--site")]
    public string? Site { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--user-dn")]
    public string? UserDn { get; set; }
}