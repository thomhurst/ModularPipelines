using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appconfig", "kv", "export")]
public record AzAppconfigKvExportOptions(
[property: CliOption("--destination")] string Destination
) : AzOptions
{
    [CliOption("--appservice-account")]
    public int? AppserviceAccount { get; set; }

    [CliOption("--auth-mode")]
    public string? AuthMode { get; set; }

    [CliOption("--connection-string")]
    public string? ConnectionString { get; set; }

    [CliOption("--dest-auth-mode")]
    public string? DestAuthMode { get; set; }

    [CliOption("--dest-connection-string")]
    public string? DestConnectionString { get; set; }

    [CliOption("--dest-endpoint")]
    public string? DestEndpoint { get; set; }

    [CliOption("--dest-label")]
    public string? DestLabel { get; set; }

    [CliOption("--dest-name")]
    public string? DestName { get; set; }

    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }

    [CliFlag("--export-as-reference")]
    public bool? ExportAsReference { get; set; }

    [CliOption("--format")]
    public string? Format { get; set; }

    [CliOption("--key")]
    public string? Key { get; set; }

    [CliOption("--label")]
    public string? Label { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--naming-convention")]
    public string? NamingConvention { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--prefix")]
    public string? Prefix { get; set; }

    [CliFlag("--preserve-labels")]
    public bool? PreserveLabels { get; set; }

    [CliOption("--profile")]
    public string? Profile { get; set; }

    [CliFlag("--resolve-keyvault")]
    public bool? ResolveKeyvault { get; set; }

    [CliOption("--separator")]
    public string? Separator { get; set; }

    [CliFlag("--skip-features")]
    public bool? SkipFeatures { get; set; }

    [CliFlag("--skip-keyvault")]
    public bool? SkipKeyvault { get; set; }

    [CliOption("--snapshot")]
    public string? Snapshot { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}