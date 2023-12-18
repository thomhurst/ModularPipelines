using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicebus", "namespace", "exists")]
public record AzServicebusNamespaceExistsOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;