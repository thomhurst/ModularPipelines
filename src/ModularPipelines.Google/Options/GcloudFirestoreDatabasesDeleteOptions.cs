using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("firestore", "databases", "delete")]
public record GcloudFirestoreDatabasesDeleteOptions(
[property: CommandSwitch("--database")] string Database
) : GcloudOptions
{
    [CommandSwitch("--etag")]
    public string? Etag { get; set; }
}