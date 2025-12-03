using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("database-migration", "connection-profiles", "update")]
public record GcloudDatabaseMigrationConnectionProfilesUpdateOptions(
[property: CliArgument] string ConnectionProfile,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliOption("--ca-certificate")]
    public string? CaCertificate { get; set; }

    [CliOption("--client-certificate")]
    public string? ClientCertificate { get; set; }

    [CliOption("--cloudsql-instance")]
    public string? CloudsqlInstance { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--host")]
    public string? Host { get; set; }

    [CliOption("--port")]
    public string? Port { get; set; }

    [CliOption("--private-key")]
    public string? PrivateKey { get; set; }

    [CliOption("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }

    [CliFlag("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CliOption("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliFlag("--prompt-for-password")]
    public bool? PromptForPassword { get; set; }
}