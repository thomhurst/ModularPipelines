using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("appconfig", "feature", "filter", "list")]
public record AzAppconfigFeatureFilterListOptions : AzOptions
{
    [CliFlag("--all")]
    public bool? All { get; set; }

    [CliOption("--auth-mode")]
    public string? AuthMode { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }

    [CliOption("--feature")]
    public string? Feature { get; set; }

    [CliOption("--key")]
    public string? Key { get; set; }

    [CliOption("--label")]
    public string? Label { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--top")]
    public string? Top { get; set; }
}