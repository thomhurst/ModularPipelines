using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("aks", "check-acr")]
public record AzAksCheckAcrOptions(
[property: CliOption("--acr")] string Acr,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--node-name")]
    public string? NodeName { get; set; }
}