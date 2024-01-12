using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "users", "list")]
public record GcloudSqlUsersListOptions(
[property: CommandSwitch("--instance")] string Instance
) : GcloudOptions;