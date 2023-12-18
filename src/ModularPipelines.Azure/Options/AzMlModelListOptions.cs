using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "model", "list")]
public record AzMlModelListOptions(
[property: CommandSwitch("--file")] string File,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--version")] string Version
) : AzOptions
{
    [BooleanCommandSwitch("--archived-only")]
    public bool? ArchivedOnly { get; set; }

    [BooleanCommandSwitch("--include-archived")]
    public bool? IncludeArchived { get; set; }

    [CommandSwitch("--max-results")]
    public string? MaxResults { get; set; }

    [CommandSwitch("--registry-name")]
    public string? RegistryName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--stage")]
    public string? Stage { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }
}

