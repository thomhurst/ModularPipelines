using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datastream", "connection-profiles", "update")]
public record GcloudDatastreamConnectionProfilesUpdateOptions(
[property: PositionalArgument] string ConnectionProfile,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--type")] string Type
) : GcloudOptions
{
    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [CommandSwitch("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CommandSwitch("--bucket")]
    public string? Bucket { get; set; }

    [CommandSwitch("--root-path")]
    public string? RootPath { get; set; }

    [CommandSwitch("--database-service")]
    public string? DatabaseService { get; set; }

    [CommandSwitch("--oracle-hostname")]
    public string? OracleHostname { get; set; }

    [CommandSwitch("--oracle-port")]
    public string? OraclePort { get; set; }

    [CommandSwitch("--oracle-username")]
    public string? OracleUsername { get; set; }

    [CommandSwitch("--oracle-password")]
    public string? OraclePassword { get; set; }

    [BooleanCommandSwitch("--oracle-prompt-for-password")]
    public bool? OraclePromptForPassword { get; set; }

    [CommandSwitch("--mysql-hostname")]
    public string? MysqlHostname { get; set; }

    [CommandSwitch("--mysql-port")]
    public string? MysqlPort { get; set; }

    [CommandSwitch("--mysql-username")]
    public string? MysqlUsername { get; set; }

    [CommandSwitch("--ca-certificate")]
    public string? CaCertificate { get; set; }

    [CommandSwitch("--client-certificate")]
    public string? ClientCertificate { get; set; }

    [CommandSwitch("--client-key")]
    public string? ClientKey { get; set; }

    [CommandSwitch("--mysql-password")]
    public string? MysqlPassword { get; set; }

    [BooleanCommandSwitch("--mysql-prompt-for-password")]
    public bool? MysqlPromptForPassword { get; set; }

    [CommandSwitch("--postgresql-database")]
    public string? PostgresqlDatabase { get; set; }

    [CommandSwitch("--postgresql-hostname")]
    public string? PostgresqlHostname { get; set; }

    [CommandSwitch("--postgresql-port")]
    public string? PostgresqlPort { get; set; }

    [CommandSwitch("--postgresql-username")]
    public string? PostgresqlUsername { get; set; }

    [CommandSwitch("--postgresql-password")]
    public string? PostgresqlPassword { get; set; }

    [BooleanCommandSwitch("--postgresql-prompt-for-password")]
    public bool? PostgresqlPromptForPassword { get; set; }

    [BooleanCommandSwitch("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CommandSwitch("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [CommandSwitch("--private-connection")]
    public string? PrivateConnection { get; set; }

    [BooleanCommandSwitch("--static-ip-connectivity")]
    public bool? StaticIpConnectivity { get; set; }

    [CommandSwitch("--forward-ssh-hostname")]
    public string? ForwardSshHostname { get; set; }

    [CommandSwitch("--forward-ssh-port")]
    public string? ForwardSshPort { get; set; }

    [CommandSwitch("--forward-ssh-username")]
    public string? ForwardSshUsername { get; set; }

    [CommandSwitch("--forward-ssh-password")]
    public string? ForwardSshPassword { get; set; }

    [CommandSwitch("--forward-ssh-private-key")]
    public string? ForwardSshPrivateKey { get; set; }
}