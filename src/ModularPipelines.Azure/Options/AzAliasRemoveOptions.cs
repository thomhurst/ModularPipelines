using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("alias", "remove")]
public record AzAliasRemoveOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;