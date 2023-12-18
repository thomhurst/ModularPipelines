using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("akshybrid", "notice")]
public record AzAkshybridNoticeOptions(
[property: CommandSwitch("--output-filepath")] string OutputFilepath
) : AzOptions
{
    [CommandSwitch("--aad-admin-group-object-ids")]
    public string? AadAdminGroupObjectIds { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

