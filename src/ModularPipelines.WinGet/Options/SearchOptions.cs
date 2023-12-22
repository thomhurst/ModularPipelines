using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.WinGet.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("search")]
public record SearchOptions(
    [property: CommandSwitch("--query")] string Query
) : WingetOptions;