using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("relay", "namespace", "exists")]
public record AzRelayNamespaceExistsOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;