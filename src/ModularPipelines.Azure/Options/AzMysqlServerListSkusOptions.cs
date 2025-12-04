using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("mysql", "server", "list-skus")]
public record AzMysqlServerListSkusOptions(
[property: CliOption("--location")] string Location
) : AzOptions;