using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("webapp", "hybrid-connection", "add")]
public record AzWebappHybridConnectionAddOptions(
[property: CliOption("--hybrid-connection")] string HybridConnection,
[property: CliOption("--name")] string Name,
[property: CliOption("--namespace")] string Namespace,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--slot")]
    public string? Slot { get; set; }
}