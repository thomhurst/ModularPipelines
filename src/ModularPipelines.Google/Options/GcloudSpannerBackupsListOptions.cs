using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spanner", "backups", "list")]
public record GcloudSpannerBackupsListOptions : GcloudOptions
{
    [CommandSwitch("--database")]
    public string? Database { get; set; }

    [CommandSwitch("--instance")]
    public string? Instance { get; set; }
}