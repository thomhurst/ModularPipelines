using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("appconfig", "kv", "set")]
public record AzAppconfigKvSetOptions(
[property: CliOption("--key")] string Key
) : AzOptions
{
    [CliOption("--auth-mode")]
    public string? AuthMode { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--content-type")]
    public string? ContentType { get; set; }

    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }

    [CliOption("--label")]
    public string? Label { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--value")]
    public string? Value { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}