using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "scope-map", "update")]
public record AzAcrScopeMapUpdateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--registry")] string Registry
) : AzOptions
{
    [CommandSwitch("--add-gateway")]
    public string? AddGateway { get; set; }

    [CommandSwitch("--add-repository")]
    public string? AddRepository { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--remove-gateway")]
    public string? RemoveGateway { get; set; }

    [CommandSwitch("--remove-repository")]
    public string? RemoveRepository { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}