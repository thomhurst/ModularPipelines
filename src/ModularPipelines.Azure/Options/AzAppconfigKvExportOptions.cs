using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appconfig", "kv", "export")]
public record AzAppconfigKvExportOptions(
[property: CommandSwitch("--destination")] string Destination
) : AzOptions
{
    [CommandSwitch("--appservice-account")]
    public int? AppserviceAccount { get; set; }

    [CommandSwitch("--auth-mode")]
    public string? AuthMode { get; set; }

    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [CommandSwitch("--dest-auth-mode")]
    public string? DestAuthMode { get; set; }

    [CommandSwitch("--dest-connection-string")]
    public string? DestConnectionString { get; set; }

    [CommandSwitch("--dest-endpoint")]
    public string? DestEndpoint { get; set; }

    [CommandSwitch("--dest-label")]
    public string? DestLabel { get; set; }

    [CommandSwitch("--dest-name")]
    public string? DestName { get; set; }

    [CommandSwitch("--endpoint")]
    public string? Endpoint { get; set; }

    [BooleanCommandSwitch("--export-as-reference")]
    public bool? ExportAsReference { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [CommandSwitch("--key")]
    public string? Key { get; set; }

    [CommandSwitch("--label")]
    public string? Label { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--naming-convention")]
    public string? NamingConvention { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--prefix")]
    public string? Prefix { get; set; }

    [BooleanCommandSwitch("--preserve-labels")]
    public bool? PreserveLabels { get; set; }

    [CommandSwitch("--profile")]
    public string? Profile { get; set; }

    [BooleanCommandSwitch("--resolve-keyvault")]
    public bool? ResolveKeyvault { get; set; }

    [CommandSwitch("--separator")]
    public string? Separator { get; set; }

    [BooleanCommandSwitch("--skip-features")]
    public bool? SkipFeatures { get; set; }

    [BooleanCommandSwitch("--skip-keyvault")]
    public bool? SkipKeyvault { get; set; }

    [CommandSwitch("--snapshot")]
    public string? Snapshot { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}