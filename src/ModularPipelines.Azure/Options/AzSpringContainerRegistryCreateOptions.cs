using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("spring", "container-registry", "create")]
public record AzSpringContainerRegistryCreateOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service
) : AzOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--server")]
    public string? Server { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }
}