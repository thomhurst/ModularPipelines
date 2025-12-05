using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("appconfig", "kv", "show")]
public record AzAppconfigKvShowOptions(
[property: CliOption("--key")] string Key
) : AzOptions
{
    [CliOption("--auth-mode")]
    public string? AuthMode { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--datetime")]
    public string? Datetime { get; set; }

    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }

    [CliOption("--label")]
    public string? Label { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }
}