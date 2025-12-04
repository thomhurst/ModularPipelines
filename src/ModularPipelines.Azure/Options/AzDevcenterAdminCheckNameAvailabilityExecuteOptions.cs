using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devcenter", "admin", "check-name-availability", "execute")]
public record AzDevcenterAdminCheckNameAvailabilityExecuteOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--type")] string Type
) : AzOptions;