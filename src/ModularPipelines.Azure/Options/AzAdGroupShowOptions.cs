using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ad", "group", "show")]
public record AzAdGroupShowOptions(
[property: CliOption("--group")] string Group
) : AzOptions;