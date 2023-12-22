using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("account", "alias", "delete")]
public record AzAccountAliasDeleteOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;