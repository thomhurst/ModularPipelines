using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dls", "fs", "access", "set-permission")]
public record AzDlsFsAccessSetPermissionOptions(
[property: CommandSwitch("--path")] string Path,
[property: CommandSwitch("--permission")] string Permission
) : AzOptions
{
    [CommandSwitch("--account")]
    public int? Account { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}