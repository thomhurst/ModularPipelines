using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("filestore", "backups", "list")]
public record GcloudFilestoreBackupsListOptions : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}