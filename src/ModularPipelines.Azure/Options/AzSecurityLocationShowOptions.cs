using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("security", "location", "show")]
public record AzSecurityLocationShowOptions(
[property: CliOption("--name")] string Name
) : AzOptions;