using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cognitiveservices", "account", "create")]
public record AzCognitiveservicesAccountCreateOptions(
[property: CommandSwitch("--kind")] string Kind,
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--sku")] string Sku
) : AzOptions
{
    [CommandSwitch("--api-properties")]
    public string? ApiProperties { get; set; }

    [BooleanCommandSwitch("--assign-identity")]
    public bool? AssignIdentity { get; set; }

    [CommandSwitch("--custom-domain")]
    public string? CustomDomain { get; set; }

    [CommandSwitch("--encryption")]
    public string? Encryption { get; set; }

    [CommandSwitch("--storage")]
    public string? Storage { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--yes")]
    public bool? Yes { get; set; } = true;
}