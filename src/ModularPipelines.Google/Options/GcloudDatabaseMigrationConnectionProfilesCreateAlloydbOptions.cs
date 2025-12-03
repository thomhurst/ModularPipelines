using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("database-migration", "connection-profiles", "create", "alloydb")]
public record GcloudDatabaseMigrationConnectionProfilesCreateAlloydbOptions(
[property: CliArgument] string ConnectionProfile,
[property: CliArgument] string Region,
[property: CliOption("--cpu-count")] string CpuCount,
[property: CliOption("--password")] string Password,
[property: CliOption("--primary-id")] string PrimaryId
) : GcloudOptions
{
    [CliFlag("--no-async")]
    public bool? NoAsync { get; set; }

    [CliOption("--cluster-labels")]
    public IEnumerable<KeyValue>? ClusterLabels { get; set; }

    [CliOption("--database-flags")]
    public string[]? DatabaseFlags { get; set; }

    [CliOption("--database-version")]
    public string? DatabaseVersion { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--primary-labels")]
    public IEnumerable<KeyValue>? PrimaryLabels { get; set; }

    [CliOption("--kms-key")]
    public string? KmsKey { get; set; }

    [CliOption("--kms-keyring")]
    public string? KmsKeyring { get; set; }

    [CliOption("--kms-project")]
    public string? KmsProject { get; set; }
}