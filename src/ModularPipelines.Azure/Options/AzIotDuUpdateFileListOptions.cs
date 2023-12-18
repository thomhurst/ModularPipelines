using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "du", "update", "file", "list")]
public record AzIotDuUpdateFileListOptions(
[property: CommandSwitch("--account")] int Account,
[property: CommandSwitch("--instance")] string Instance,
[property: CommandSwitch("--un")] string Un,
[property: CommandSwitch("--up")] string Up,
[property: CommandSwitch("--update-version")] string UpdateVersion
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}

