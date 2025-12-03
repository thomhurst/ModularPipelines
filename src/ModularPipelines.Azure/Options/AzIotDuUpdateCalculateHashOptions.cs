using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "du", "update", "calculate-hash")]
public record AzIotDuUpdateCalculateHashOptions(
[property: CliOption("--file-path")] string FilePath
) : AzOptions
{
    [CliOption("--hash-algo")]
    public string? HashAlgo { get; set; }
}