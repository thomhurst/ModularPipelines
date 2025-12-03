using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("relay", "namespace", "exists")]
public record AzRelayNamespaceExistsOptions(
[property: CliOption("--name")] string Name
) : AzOptions;