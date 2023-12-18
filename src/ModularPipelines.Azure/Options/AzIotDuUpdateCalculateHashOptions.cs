using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "du", "update", "calculate-hash")]
public record AzIotDuUpdateCalculateHashOptions(
[property: CommandSwitch("--file-path")] string FilePath
) : AzOptions
{
    [CommandSwitch("--hash-algo")]
    public string? HashAlgo { get; set; }
}