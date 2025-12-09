using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("security", "contact", "show")]
public record AzSecurityContactShowOptions(
[property: CliOption("--name")] string Name
) : AzOptions;