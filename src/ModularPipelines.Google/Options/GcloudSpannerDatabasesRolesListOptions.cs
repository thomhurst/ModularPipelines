using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spanner", "databases", "roles", "list")]
public record GcloudSpannerDatabasesRolesListOptions(
[property: CommandSwitch("--database")] string Database,
[property: CommandSwitch("--instance")] string Instance
) : GcloudOptions;