using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "component", "list")]
public record AzMlComponentListOptions : AzOptions
{
    [BooleanCommandSwitch("--archived-only")]
    public bool? ArchivedOnly { get; set; }

    [BooleanCommandSwitch("--include-archived")]
    public bool? IncludeArchived { get; set; }

    [CommandSwitch("--max-results")]
    public string? MaxResults { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--registry-name")]
    public string? RegistryName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }
}