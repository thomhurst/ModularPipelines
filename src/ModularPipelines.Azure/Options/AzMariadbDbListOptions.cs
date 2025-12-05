using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("mariadb", "db", "list")]
public record AzMariadbDbListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--server-name")] string ServerName
) : AzOptions;