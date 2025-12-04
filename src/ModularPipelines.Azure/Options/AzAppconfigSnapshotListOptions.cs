using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("appconfig", "snapshot", "list")]
public record AzAppconfigSnapshotListOptions : AzOptions
{
    [CliOption("--all")]
    public string? All { get; set; }

    [CliOption("--auth-mode")]
    public string? AuthMode { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }

    [CliOption("--fields")]
    public string? Fields { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--snapshot-name")]
    public string? SnapshotName { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--top")]
    public string? Top { get; set; }
}