using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("keyvault", "list-deleted")]
public record AzKeyvaultListDeletedOptions : AzOptions
{
    [CommandSwitch("--resource-type")]
    public string? ResourceType { get; set; }
}