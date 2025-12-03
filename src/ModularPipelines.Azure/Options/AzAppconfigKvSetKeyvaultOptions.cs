using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appconfig", "kv", "set-keyvault")]
public record AzAppconfigKvSetKeyvaultOptions(
[property: CliOption("--key")] string Key,
[property: CliOption("--secret-identifier")] string SecretIdentifier
) : AzOptions
{
    [CliOption("--auth-mode")]
    public string? AuthMode { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }

    [CliOption("--label")]
    public string? Label { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}