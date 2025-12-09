using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("spring", "build-service", "builder", "buildpack-binding", "list")]
public record AzSpringBuildServiceBuilderBuildpackBindingListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service
) : AzOptions
{
    [CliOption("--builder-name")]
    public string? BuilderName { get; set; }
}