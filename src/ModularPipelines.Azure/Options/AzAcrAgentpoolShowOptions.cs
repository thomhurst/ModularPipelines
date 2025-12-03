using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acr", "agentpool", "show")]
public record AzAcrAgentpoolShowOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--registry")] string Registry
) : AzOptions
{
    [CliFlag("--queue-count")]
    public bool? QueueCount { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}