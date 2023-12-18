using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("account", "alias", "show")]
public record AzAccountAliasShowOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;