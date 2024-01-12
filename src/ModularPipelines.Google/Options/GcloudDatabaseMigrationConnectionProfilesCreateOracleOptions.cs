using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("database-migration", "connection-profiles", "create", "oracle")]
public record GcloudDatabaseMigrationConnectionProfilesCreateOracleOptions(
[property: PositionalArgument] string ConnectionProfile,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--database-service")] string DatabaseService,
[property: CommandSwitch("--host")] string Host,
[property: CommandSwitch("--port")] string Port,
[property: CommandSwitch("--username")] string Username,
[property: CommandSwitch("--password")] string Password,
[property: BooleanCommandSwitch("--prompt-for-password")] bool PromptForPassword
) : GcloudOptions
{
    [BooleanCommandSwitch("--no-async")]
    public bool? NoAsync { get; set; }

    [CommandSwitch("--ca-certificate")]
    public string? CaCertificate { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--private-connection")]
    public string? PrivateConnection { get; set; }

    [BooleanCommandSwitch("--static-ip-connectivity")]
    public bool? StaticIpConnectivity { get; set; }

    [CommandSwitch("--forward-ssh-hostname")]
    public string? ForwardSshHostname { get; set; }

    [CommandSwitch("--forward-ssh-username")]
    public string? ForwardSshUsername { get; set; }

    [CommandSwitch("--forward-ssh-port")]
    public string? ForwardSshPort { get; set; }

    [CommandSwitch("--forward-ssh-password")]
    public string? ForwardSshPassword { get; set; }

    [CommandSwitch("--forward-ssh-private-key")]
    public string? ForwardSshPrivateKey { get; set; }
}