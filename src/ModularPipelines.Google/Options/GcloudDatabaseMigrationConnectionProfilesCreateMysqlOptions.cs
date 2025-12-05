using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("database-migration", "connection-profiles", "create", "mysql")]
public record GcloudDatabaseMigrationConnectionProfilesCreateMysqlOptions(
[property: CliArgument] string ConnectionProfile,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliFlag("--no-async")]
    public bool? NoAsync { get; set; }

    [CliOption("--cloudsql-instance")]
    public string? CloudsqlInstance { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--provider")]
    public string? Provider { get; set; }

    [CliOption("--ca-certificate")]
    public string? CaCertificate { get; set; }

    [CliOption("--client-certificate")]
    public string? ClientCertificate { get; set; }

    [CliOption("--private-key")]
    public string? PrivateKey { get; set; }

    [CliOption("--host")]
    public string? Host { get; set; }

    [CliOption("--port")]
    public string? Port { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliFlag("--prompt-for-password")]
    public bool? PromptForPassword { get; set; }
}