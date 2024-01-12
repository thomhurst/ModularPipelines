using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spanner", "databases", "ddl", "update")]
public record GcloudSpannerDatabasesDdlUpdateOptions(
[property: PositionalArgument] string Database,
[property: PositionalArgument] string Instance
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--ddl")]
    public string? Ddl { get; set; }

    [CommandSwitch("--ddl-file")]
    public string? DdlFile { get; set; }
}