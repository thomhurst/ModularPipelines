using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("acr", "scope-map", "create")]
public record AzAcrScopeMapCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--registry")] string Registry
) : AzOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--gateway")]
    public string? Gateway { get; set; }

    [CommandSwitch("--repository")]
    public string? Repository { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}