using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("security", "location", "show")]
public record AzSecurityLocationShowOptions(
[property: CliOption("--name")] string Name
) : AzOptions;