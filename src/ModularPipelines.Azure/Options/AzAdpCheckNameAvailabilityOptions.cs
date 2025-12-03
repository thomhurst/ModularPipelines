using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("adp", "check-name-availability")]
public record AzAdpCheckNameAvailabilityOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--type")] string Type
) : AzOptions;