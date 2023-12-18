using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("account", "set")]
public record AzAccountSetOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions;