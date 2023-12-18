using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("graph", "shared-query", "create")]
public record AzGraphSharedQueryCreateOptions(
[property: CommandSwitch("--description")] string Description,
[property: CommandSwitch("--graph-query")] string GraphQuery,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}