using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "du", "update", "file", "show")]
public record AzIotDuUpdateFileShowOptions(
[property: CommandSwitch("--account")] int Account,
[property: CommandSwitch("--instance")] string Instance,
[property: CommandSwitch("--ufid")] string Ufid,
[property: CommandSwitch("--un")] string Un,
[property: CommandSwitch("--up")] string Up,
[property: CommandSwitch("--update-version")] string UpdateVersion
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}