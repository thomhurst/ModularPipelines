using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("database-migration", "connection-profiles", "create", "oracle")]
public record GcloudDatabaseMigrationConnectionProfilesCreateOracleOptions(
[property: CliArgument] string ConnectionProfile,
[property: CliArgument] string Region,
[property: CliOption("--database-service")] string DatabaseService,
[property: CliOption("--host")] string Host,
[property: CliOption("--port")] string Port,
[property: CliOption("--username")] string Username,
[property: CliOption("--password")] string Password,
[property: CliFlag("--prompt-for-password")] bool PromptForPassword
) : GcloudOptions
{
    [CliFlag("--no-async")]
    public bool? NoAsync { get; set; }

    [CliOption("--ca-certificate")]
    public string? CaCertificate { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

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