using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("provider", "unregister")]
public record AzProviderUnregisterOptions(
[property: CliOption("--namespace")] string Namespace
) : AzOptions
{
    [CliFlag("--wait")]
    public bool? Wait { get; set; }
}