using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dls", "fs", "access", "remove-all")]
public record AzDlsFsAccessRemoveAllOptions(
[property: CommandSwitch("--path")] string Path
) : AzOptions
{
    [CommandSwitch("--account")]
    public int? Account { get; set; }

    [BooleanCommandSwitch("--default-acl")]
    public bool? DefaultAcl { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}