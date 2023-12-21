using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netappfiles", "account", "ad", "add")]
public record AzNetappfilesAccountAdAddOptions(
[property: CommandSwitch("--dns")] string Dns,
[property: CommandSwitch("--domain")] string Domain,
[property: CommandSwitch("--password")] string Password,
[property: CommandSwitch("--smb-server-name")] string SmbServerName,
[property: CommandSwitch("--username")] string Username
) : AzOptions
{
    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--ad-name")]
    public string? AdName { get; set; }

    [CommandSwitch("--administrators")]
    public string? Administrators { get; set; }

    [BooleanCommandSwitch("--aes-encryption")]
    public bool? AesEncryption { get; set; }

    [BooleanCommandSwitch("--allow-local-ldap-users")]
    public bool? AllowLocalLdapUsers { get; set; }

    [CommandSwitch("--backup-operators")]
    public string? BackupOperators { get; set; }

    [BooleanCommandSwitch("--encrypt-dc-conn")]
    public bool? EncryptDcConn { get; set; }

    [CommandSwitch("--group-dn")]
    public string? GroupDn { get; set; }

    [CommandSwitch("--group-filter")]
    public string? GroupFilter { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--kdc-ip")]
    public string? KdcIp { get; set; }

    [CommandSwitch("--ldap-over-tls")]
    public string? LdapOverTls { get; set; }

    [BooleanCommandSwitch("--ldap-signing")]
    public bool? LdapSigning { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--organizational-unit")]
    public string? OrganizationalUnit { get; set; }

    [CommandSwitch("--preferred-servers-for-ldap-client")]
    public string? PreferredServersForLdapClient { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--security-operators")]
    public string? SecurityOperators { get; set; }

    [CommandSwitch("--server-root-ca-cert")]
    public string? ServerRootCaCert { get; set; }

    [CommandSwitch("--site")]
    public string? Site { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--user-dn")]
    public string? UserDn { get; set; }
}