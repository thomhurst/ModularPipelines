using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("spring", "build-service", "build", "result", "show")]
public record AzSpringBuildServiceBuildResultShowOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service
) : AzOptions
{
    [CliOption("--build-name")]
    public string? BuildName { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }
}