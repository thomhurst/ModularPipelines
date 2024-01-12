using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "flags", "list")]
public record GcloudSqlFlagsListOptions : GcloudOptions
{
    [CommandSwitch("--database-version")]
    public string? DatabaseVersion { get; set; }
}