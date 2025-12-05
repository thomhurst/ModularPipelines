using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("appconfig", "snapshot", "archive")]
public record AzAppconfigSnapshotArchiveOptions(
[property: CliOption("--snapshot-name")] string SnapshotName
) : AzOptions
{
    [CliOption("--auth-mode")]
    public string? AuthMode { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }
}