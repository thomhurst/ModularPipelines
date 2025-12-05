using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "users", "list")]
public record GcloudSqlUsersListOptions(
[property: CliOption("--instance")] string Instance
) : GcloudOptions;