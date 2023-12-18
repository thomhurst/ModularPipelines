using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("alias", "import")]
public record AzAliasImportOptions(
[property: CommandSwitch("--source")] string Source
) : AzOptions;