using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eventhubs", "namespace", "exists")]
public record AzEventhubsNamespaceExistsOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;