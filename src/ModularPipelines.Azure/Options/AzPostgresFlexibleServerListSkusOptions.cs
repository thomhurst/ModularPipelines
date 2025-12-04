using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("postgres", "flexible-server", "list-skus")]
public record AzPostgresFlexibleServerListSkusOptions(
[property: CliOption("--location")] string Location
) : AzOptions;