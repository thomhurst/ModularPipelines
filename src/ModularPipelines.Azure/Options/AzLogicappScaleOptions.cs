using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logicapp", "scale")]
public record AzLogicappScaleOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--max-instances")]
    public string? MaxInstances { get; set; }

    [CliOption("--min-instances")]
    public string? MinInstances { get; set; }

    [CliOption("--slot")]
    public string? Slot { get; set; }
}