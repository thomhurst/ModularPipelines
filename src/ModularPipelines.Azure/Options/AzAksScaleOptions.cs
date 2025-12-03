using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("aks", "scale")]
public record AzAksScaleOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--node-count")] int NodeCount,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--nodepool-name")]
    public string? NodepoolName { get; set; }
}