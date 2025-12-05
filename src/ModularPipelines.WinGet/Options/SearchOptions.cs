using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.WinGet.Options;

[ExcludeFromCodeCoverage]
[CliCommand("search")]
public record SearchOptions(
    [property: CliOption("--query")] string Query
) : WingetOptions;