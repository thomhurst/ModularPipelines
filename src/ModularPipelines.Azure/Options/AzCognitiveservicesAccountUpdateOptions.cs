using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cognitiveservices", "account", "update")]
public record AzCognitiveservicesAccountUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--api-properties")]
    public string? ApiProperties { get; set; }

    [CliOption("--custom-domain")]
    public string? CustomDomain { get; set; }

    [CliOption("--encryption")]
    public string? Encryption { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--storage")]
    public string? Storage { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}