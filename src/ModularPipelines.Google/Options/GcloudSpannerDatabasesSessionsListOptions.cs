using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spanner", "databases", "sessions", "list")]
public record GcloudSpannerDatabasesSessionsListOptions(
[property: CommandSwitch("--database")] string Database,
[property: CommandSwitch("--instance")] string Instance
) : GcloudOptions
{
    [CommandSwitch("--server-filter")]
    public string? ServerFilter { get; set; }
}