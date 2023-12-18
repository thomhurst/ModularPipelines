using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stack", "group", "list")]
public record AzStackGroupListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;