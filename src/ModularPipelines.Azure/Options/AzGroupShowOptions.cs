using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("group", "show")]
public record AzGroupShowOptions(
[property: CliOption("--name")] string Name
) : AzOptions;