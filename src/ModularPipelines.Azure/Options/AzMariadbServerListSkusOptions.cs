using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("mariadb", "server", "list-skus")]
public record AzMariadbServerListSkusOptions(
[property: CliOption("--location")] string Location
) : AzOptions;