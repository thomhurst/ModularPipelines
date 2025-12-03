using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spring", "app", "stop")]
public record AzSpringAppStopOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service
) : AzOptions
{
    [CliOption("--deployment")]
    public string? Deployment { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}