using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appconfig", "kv", "import")]
public record AzAppconfigKvImportOptions(
[property: CommandSwitch("--source")] string Source
) : AzOptions
{
    [CommandSwitch("--appservice-account")]
    public int? AppserviceAccount { get; set; }

    [CommandSwitch("--auth-mode")]
    public string? AuthMode { get; set; }

    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [CommandSwitch("--content-type")]
    public string? ContentType { get; set; }

    [CommandSwitch("--depth")]
    public string? Depth { get; set; }

    [CommandSwitch("--endpoint")]
    public string? Endpoint { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [CommandSwitch("--import-mode")]
    public string? ImportMode { get; set; }

    [CommandSwitch("--label")]
    public string? Label { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--prefix")]
    public string? Prefix { get; set; }

    [BooleanCommandSwitch("--preserve-labels")]
    public bool? PreserveLabels { get; set; }

    [CommandSwitch("--profile")]
    public string? Profile { get; set; }

    [CommandSwitch("--separator")]
    public string? Separator { get; set; }

    [BooleanCommandSwitch("--skip-features")]
    public bool? SkipFeatures { get; set; }

    [CommandSwitch("--src-auth-mode")]
    public string? SrcAuthMode { get; set; }

    [CommandSwitch("--src-connection-string")]
    public string? SrcConnectionString { get; set; }

    [CommandSwitch("--src-endpoint")]
    public string? SrcEndpoint { get; set; }

    [CommandSwitch("--src-key")]
    public string? SrcKey { get; set; }

    [CommandSwitch("--src-label")]
    public string? SrcLabel { get; set; }

    [CommandSwitch("--src-name")]
    public string? SrcName { get; set; }

    [CommandSwitch("--src-snapshot")]
    public string? SrcSnapshot { get; set; }

    [BooleanCommandSwitch("--strict")]
    public bool? Strict { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}

