using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("appconfig", "kv", "restore")]
public record AzAppconfigKvRestoreOptions(
[property: CliOption("--datetime")] string Datetime
) : AzOptions
{
    [CliOption("--auth-mode")]
    public string? AuthMode { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }

    [CliOption("--key")]
    public string? Key { get; set; }

    [CliOption("--label")]
    public string? Label { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}