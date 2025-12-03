using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spring", "test-endpoint", "list")]
public record AzSpringTestEndpointListOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--app")]
    public string? App { get; set; }

    [CliOption("--deployment")]
    public string? Deployment { get; set; }
}