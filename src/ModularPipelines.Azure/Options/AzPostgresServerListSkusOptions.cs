using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("postgres", "server", "list-skus")]
public record AzPostgresServerListSkusOptions(
[property: CliOption("--location")] string Location
) : AzOptions;