using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("netapp", "active-directories", "create")]
public record GcloudNetappActiveDirectoriesCreateOptions(
[property: CliArgument] string ActiveDirectory,
[property: CliArgument] string Location,
[property: CliOption("--dns")] string Dns,
[property: CliOption("--domain")] string Domain,
[property: CliOption("--net-bios-prefix")] string NetBiosPrefix,
[property: CliOption("--password")] string Password,
[property: CliOption("--username")] string Username
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--backup-operators")]
    public string[]? BackupOperators { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--enable-aes")]
    public string? EnableAes { get; set; }

    [CliOption("--enable-ldap-signing")]
    public string? EnableLdapSigning { get; set; }

    [CliOption("--encrypt-dc-connections")]
    public string? EncryptDcConnections { get; set; }

    [CliOption("--kdc-hostname")]
    public string? KdcHostname { get; set; }

    [CliOption("--kdc-ip")]
    public string? KdcIp { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--nfs-users-with-ldap")]
    public string? NfsUsersWithLdap { get; set; }

    [CliOption("--organizational-unit")]
    public string? OrganizationalUnit { get; set; }

    [CliOption("--security-operators")]
    public string[]? SecurityOperators { get; set; }

    [CliOption("--site")]
    public string? Site { get; set; }
}