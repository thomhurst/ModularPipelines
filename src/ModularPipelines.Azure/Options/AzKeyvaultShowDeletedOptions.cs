using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "show-deleted")]
public record AzKeyvaultShowDeletedOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--hsm-name")]
    public string? HsmName { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }
}

