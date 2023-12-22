using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "group", "delete")]
public record AzAdGroupDeleteOptions(
[property: CommandSwitch("--group")] string Group
) : AzOptions;