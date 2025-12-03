using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("hack", "show")]
public record AzHackShowOptions(
[property: CliOption("--name")] string Name
) : AzOptions;