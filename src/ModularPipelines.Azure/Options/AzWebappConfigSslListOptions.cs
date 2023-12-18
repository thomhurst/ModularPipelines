using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("webapp", "config", "ssl", "list")]
public record AzWebappConfigSslListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}