using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("graph", "shared-query", "list")]
public record AzGraphSharedQueryListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
}