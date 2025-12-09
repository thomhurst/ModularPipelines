using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("spring", "dev-tool", "update")]
public record AzSpringDevToolUpdateOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service")] string Service
) : AzOptions
{
    [CliFlag("--assign-endpoint")]
    public bool? AssignEndpoint { get; set; }

    [CliOption("--client-id")]
    public string? ClientId { get; set; }

    [CliOption("--client-secret")]
    public string? ClientSecret { get; set; }

    [CliOption("--metadata-url")]
    public string? MetadataUrl { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--scopes")]
    public string? Scopes { get; set; }
}