using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognitiveservices", "account", "show-deleted")]
public record AzCognitiveservicesAccountShowDeletedOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--api-properties")]
    public string? ApiProperties { get; set; }

    [CommandSwitch("--custom-domain")]
    public string? CustomDomain { get; set; }

    [CommandSwitch("--encryption")]
    public string? Encryption { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--storage")]
    public string? Storage { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}