using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("mysql", "flexible-server", "ad-admin", "list")]
public record AzMysqlFlexibleServerAdAdminListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--server-name")] string ServerName
) : AzOptions;