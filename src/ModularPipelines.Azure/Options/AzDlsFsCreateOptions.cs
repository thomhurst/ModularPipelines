using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dls", "fs", "create")]
public record AzDlsFsCreateOptions(
[property: CommandSwitch("--account")] int Account,
[property: CommandSwitch("--path")] string Path
) : AzOptions
{
    [CommandSwitch("--content")]
    public string? Content { get; set; }

    [BooleanCommandSwitch("--folder")]
    public bool? Folder { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }
}