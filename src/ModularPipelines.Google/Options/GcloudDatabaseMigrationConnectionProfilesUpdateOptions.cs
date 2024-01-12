using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("database-migration", "connection-profiles", "update")]
public record GcloudDatabaseMigrationConnectionProfilesUpdateOptions(
[property: PositionalArgument] string ConnectionProfile,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [CommandSwitch("--ca-certificate")]
    public string? CaCertificate { get; set; }

    [CommandSwitch("--client-certificate")]
    public string? ClientCertificate { get; set; }

    [CommandSwitch("--cloudsql-instance")]
    public string? CloudsqlInstance { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--host")]
    public string? Host { get; set; }

    [CommandSwitch("--port")]
    public string? Port { get; set; }

    [CommandSwitch("--private-key")]
    public string? PrivateKey { get; set; }

    [CommandSwitch("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CommandSwitch("--username")]
    public string? Username { get; set; }

    [BooleanCommandSwitch("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CommandSwitch("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [BooleanCommandSwitch("--prompt-for-password")]
    public bool? PromptForPassword { get; set; }
}