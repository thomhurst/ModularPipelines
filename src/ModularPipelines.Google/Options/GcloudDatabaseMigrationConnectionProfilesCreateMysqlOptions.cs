using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("database-migration", "connection-profiles", "create", "mysql")]
public record GcloudDatabaseMigrationConnectionProfilesCreateMysqlOptions(
[property: PositionalArgument] string ConnectionProfile,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [BooleanCommandSwitch("--no-async")]
    public bool? NoAsync { get; set; }

    [CommandSwitch("--cloudsql-instance")]
    public string? CloudsqlInstance { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--provider")]
    public string? Provider { get; set; }

    [CommandSwitch("--ca-certificate")]
    public string? CaCertificate { get; set; }

    [CommandSwitch("--client-certificate")]
    public string? ClientCertificate { get; set; }

    [CommandSwitch("--private-key")]
    public string? PrivateKey { get; set; }

    [CommandSwitch("--host")]
    public string? Host { get; set; }

    [CommandSwitch("--port")]
    public string? Port { get; set; }

    [CommandSwitch("--username")]
    public string? Username { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [BooleanCommandSwitch("--prompt-for-password")]
    public bool? PromptForPassword { get; set; }
}