using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("servicebus", "namespace", "exists")]
public record AzServicebusNamespaceExistsOptions(
[property: CliOption("--name")] string Name
) : AzOptions;