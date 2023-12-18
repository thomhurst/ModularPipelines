using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appconfig", "feature", "filter", "add")]
public record AzAppconfigFeatureFilterAddOptions(
[property: CommandSwitch("--filter-name")] string FilterName
) : AzOptions
{
    [CommandSwitch("--auth-mode")]
    public string? AuthMode { get; set; }

    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [CommandSwitch("--endpoint")]
    public string? Endpoint { get; set; }

    [CommandSwitch("--feature")]
    public string? Feature { get; set; }

    [CommandSwitch("--filter-parameters")]
    public string? FilterParameters { get; set; }

    [CommandSwitch("--index")]
    public string? Index { get; set; }

    [CommandSwitch("--key")]
    public string? Key { get; set; }

    [CommandSwitch("--label")]
    public string? Label { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}