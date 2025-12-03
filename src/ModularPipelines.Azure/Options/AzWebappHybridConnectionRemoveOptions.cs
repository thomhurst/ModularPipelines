using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("webapp", "hybrid-connection", "remove")]
public record AzWebappHybridConnectionRemoveOptions(
[property: CliOption("--hybrid-connection")] string HybridConnection,
[property: CliOption("--name")] string Name,
[property: CliOption("--namespace")] string Namespace,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--slot")]
    public string? Slot { get; set; }
}