using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("netapp", "active-directories", "update")]
public record GcloudNetappActiveDirectoriesUpdateOptions(
[property: PositionalArgument] string ActiveDirectory,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--dns")] string Dns,
[property: CommandSwitch("--domain")] string Domain,
[property: CommandSwitch("--net-bios-prefix")] string NetBiosPrefix,
[property: CommandSwitch("--password")] string Password,
[property: CommandSwitch("--username")] string Username
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--backup-operators")]
    public string[]? BackupOperators { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--enable-aes")]
    public string? EnableAes { get; set; }

    [CommandSwitch("--enable-ldap-signing")]
    public string? EnableLdapSigning { get; set; }

    [CommandSwitch("--encrypt-dc-connections")]
    public string? EncryptDcConnections { get; set; }

    [CommandSwitch("--kdc-hostname")]
    public string? KdcHostname { get; set; }

    [CommandSwitch("--kdc-ip")]
    public string? KdcIp { get; set; }

    [CommandSwitch("--nfs-users-with-ldap")]
    public string? NfsUsersWithLdap { get; set; }

    [CommandSwitch("--organizational-unit")]
    public string? OrganizationalUnit { get; set; }

    [CommandSwitch("--security-operators")]
    public string[]? SecurityOperators { get; set; }

    [CommandSwitch("--site")]
    public string? Site { get; set; }

    [CommandSwitch("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [BooleanCommandSwitch("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CommandSwitch("--remove-labels")]
    public string[]? RemoveLabels { get; set; }
}