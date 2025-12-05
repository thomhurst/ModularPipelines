using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("mysql", "flexible-server", "db", "list")]
public record AzMysqlFlexibleServerDbListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--server-name")] string ServerName
) : AzOptions;