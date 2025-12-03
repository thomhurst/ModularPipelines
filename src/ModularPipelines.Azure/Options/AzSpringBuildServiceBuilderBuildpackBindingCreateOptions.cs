using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spring", "build-service", "builder", "buildpack-binding", "create")]
public record AzSpringBuildServiceBuilderBuildpackBindingCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service,
[property: CliOption("--type")] string Type
) : AzOptions
{
    [CliOption("--builder-name")]
    public string? BuilderName { get; set; }

    [CliOption("--properties")]
    public string? Properties { get; set; }

    [CliOption("--secrets")]
    public string? Secrets { get; set; }
}