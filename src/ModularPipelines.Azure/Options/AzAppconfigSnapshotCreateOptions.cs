using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("appconfig", "snapshot", "create")]
public record AzAppconfigSnapshotCreateOptions(
[property: CliOption("--filters")] string Filters,
[property: CliOption("--snapshot-name")] string SnapshotName
) : AzOptions
{
    [CliOption("--auth-mode")]
    public string? AuthMode { get; set; }

    [CliOption("--composition-type")]
    public string? CompositionType { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--retention-period")]
    public string? RetentionPeriod { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}