using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("group", "show")]
public record AzGroupShowOptions(
[property: CliOption("--name")] string Name
) : AzOptions;