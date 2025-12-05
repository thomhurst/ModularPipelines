using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("mysql", "flexible-server", "list-skus")]
public record AzMysqlFlexibleServerListSkusOptions(
[property: CliOption("--location")] string Location
) : AzOptions;