using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup-dr", "management-servers", "list")]
public record GcloudBackupDrManagementServersListOptions : GcloudOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }
}