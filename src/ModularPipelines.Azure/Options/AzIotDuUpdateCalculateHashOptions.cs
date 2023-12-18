using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "du", "update", "calculate-hash")]
public record AzIotDuUpdateCalculateHashOptions(
[property: CommandSwitch("--file-path")] string FilePath
) : AzOptions
{
    [CommandSwitch("--hash-algo")]
    public string? HashAlgo { get; set; }
}