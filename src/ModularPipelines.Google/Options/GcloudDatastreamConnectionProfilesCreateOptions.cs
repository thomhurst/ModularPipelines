using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datastream", "connection-profiles", "create")]
public record GcloudDatastreamConnectionProfilesCreateOptions(
[property: CliArgument] string ConnectionProfile,
[property: CliArgument] string Location,
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--type")] string Type
) : GcloudOptions
{
    [CliFlag("--force")]
    public bool? Force { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--bucket")]
    public string? Bucket { get; set; }

    [CliOption("--root-path")]
    public string? RootPath { get; set; }

    [CliOption("--database-service")]
    public string? DatabaseService { get; set; }

    [CliOption("--oracle-hostname")]
    public string? OracleHostname { get; set; }

    [CliOption("--oracle-port")]
    public string? OraclePort { get; set; }

    [CliOption("--oracle-username")]
    public string? OracleUsername { get; set; }

    [CliOption("--oracle-password")]
    public string? OraclePassword { get; set; }

    [CliFlag("--oracle-prompt-for-password")]
    public bool? OraclePromptForPassword { get; set; }

    [CliOption("--mysql-hostname")]
    public string? MysqlHostname { get; set; }

    [CliOption("--mysql-port")]
    public string? MysqlPort { get; set; }

    [CliOption("--mysql-username")]
    public string? MysqlUsername { get; set; }

    [CliOption("--mysql-password")]
    public string? MysqlPassword { get; set; }

    [CliFlag("--mysql-prompt-for-password")]
    public bool? MysqlPromptForPassword { get; set; }

    [CliOption("--ca-certificate")]
    public string? CaCertificate { get; set; }

    [CliOption("--client-certificate")]
    public string? ClientCertificate { get; set; }

    [CliOption("--client-key")]
    public string? ClientKey { get; set; }

    [CliOption("--postgresql-database")]
    public string? PostgresqlDatabase { get; set; }

    [CliOption("--postgresql-hostname")]
    public string? PostgresqlHostname { get; set; }

    [CliOption("--postgresql-port")]
    public string? PostgresqlPort { get; set; }

    [CliOption("--postgresql-username")]
    public string? PostgresqlUsername { get; set; }

    [CliOption("--postgresql-password")]
    public string? PostgresqlPassword { get; set; }

    [CliFlag("--postgresql-prompt-for-password")]
    public bool? PostgresqlPromptForPassword { get; set; }

    [CliOption("--private-connection")]
    public string? PrivateConnection { get; set; }

    [CliFlag("--static-ip-connectivity")]
    public bool? StaticIpConnectivity { get; set; }

    [CliOption("--forward-ssh-hostname")]
    public string? ForwardSshHostname { get; set; }

    [CliOption("--forward-ssh-username")]
    public string? ForwardSshUsername { get; set; }

    [CliOption("--forward-ssh-port")]
    public string? ForwardSshPort { get; set; }

    [CliOption("--forward-ssh-password")]
    public string? ForwardSshPassword { get; set; }

    [CliOption("--forward-ssh-private-key")]
    public string? ForwardSshPrivateKey { get; set; }
}