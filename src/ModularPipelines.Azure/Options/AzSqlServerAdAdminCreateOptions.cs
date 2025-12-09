using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "server", "ad-admin", "create")]
public record AzSqlServerAdAdminCreateOptions(
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--object-id")] string ObjectId,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--server")] string Server
) : AzOptions;