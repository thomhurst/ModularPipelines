using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("keyvault", "check-name")]
public record AzKeyvaultCheckNameOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }
}