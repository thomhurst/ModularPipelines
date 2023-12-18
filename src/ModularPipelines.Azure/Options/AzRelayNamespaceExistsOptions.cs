using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("relay", "namespace", "exists")]
public record AzRelayNamespaceExistsOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
}