using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dt", "model", "list")]
public record AzDtModelListOptions(
[property: CommandSwitch("--dt-name")] string DtName
) : AzOptions
{
    [BooleanCommandSwitch("--def")]
    public bool? Def { get; set; }

    [CommandSwitch("--dependencies-for")]
    public string? DependenciesFor { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}

