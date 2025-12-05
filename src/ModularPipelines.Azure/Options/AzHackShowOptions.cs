using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("hack", "show")]
public record AzHackShowOptions(
[property: CliOption("--name")] string Name
) : AzOptions;