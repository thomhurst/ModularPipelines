using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mariadb", "server", "list-skus")]
public record AzMariadbServerListSkusOptions(
[property: CliOption("--location")] string Location
) : AzOptions;