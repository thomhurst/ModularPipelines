using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("appconfig", "kv", "import")]
public record AzAppconfigKvImportOptions(
[property: CliOption("--source")] string Source
) : AzOptions
{
    [CliOption("--appservice-account")]
    public int? AppserviceAccount { get; set; }

    [CliOption("--auth-mode")]
    public string? AuthMode { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--content-type")]
    public string? ContentType { get; set; }

    [CliOption("--depth")]
    public string? Depth { get; set; }

    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }

    [CliOption("--format")]
    public string? Format { get; set; }

    [CliOption("--import-mode")]
    public string? ImportMode { get; set; }

    [CliOption("--label")]
    public string? Label { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--prefix")]
    public string? Prefix { get; set; }

    [CliFlag("--preserve-labels")]
    public bool? PreserveLabels { get; set; }

    [CliOption("--profile")]
    public string? Profile { get; set; }

    [CliOption("--separator")]
    public string? Separator { get; set; }

    [CliFlag("--skip-features")]
    public bool? SkipFeatures { get; set; }

    [CliOption("--src-auth-mode")]
    public string? SrcAuthMode { get; set; }

    [CliOption("--src-connection-string")]
    public string? SrcConnectionString { get; set; }

    [CliOption("--src-endpoint")]
    public string? SrcEndpoint { get; set; }

    [CliOption("--src-key")]
    public string? SrcKey { get; set; }

    [CliOption("--src-label")]
    public string? SrcLabel { get; set; }

    [CliOption("--src-name")]
    public string? SrcName { get; set; }

    [CliOption("--src-snapshot")]
    public string? SrcSnapshot { get; set; }

    [CliFlag("--strict")]
    public bool? Strict { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}