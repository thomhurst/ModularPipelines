using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("cognitiveservices", "account", "create")]
public record AzCognitiveservicesAccountCreateOptions(
[property: CliOption("--kind")] string Kind,
[property: CliOption("--location")] string Location,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--sku")] string Sku
) : AzOptions
{
    [CliOption("--api-properties")]
    public string? ApiProperties { get; set; }

    [CliFlag("--assign-identity")]
    public bool? AssignIdentity { get; set; }

    [CliOption("--custom-domain")]
    public string? CustomDomain { get; set; }

    [CliOption("--encryption")]
    public string? Encryption { get; set; }

    [CliOption("--storage")]
    public string? Storage { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--yes")]
    public bool? Yes { get; set; } = true;
}