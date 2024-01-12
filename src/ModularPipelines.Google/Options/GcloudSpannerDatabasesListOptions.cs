using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spanner", "databases", "list")]
public record GcloudSpannerDatabasesListOptions : GcloudOptions
{
    [CommandSwitch("--instance")]
    public string? Instance { get; set; }
}