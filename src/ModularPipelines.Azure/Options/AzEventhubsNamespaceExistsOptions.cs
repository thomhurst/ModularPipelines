using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("eventhubs", "namespace", "exists")]
public record AzEventhubsNamespaceExistsOptions(
[property: CliOption("--name")] string Name
) : AzOptions;