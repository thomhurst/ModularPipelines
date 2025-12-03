using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventhubs", "namespace", "exists")]
public record AzEventhubsNamespaceExistsOptions(
[property: CliOption("--name")] string Name
) : AzOptions;