using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("staticwebapp", "backends", "link")]
public record AzStaticwebappBackendsLinkOptions(
[property: CliOption("--backend-resource-id")] string BackendResourceId,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--backend-region")]
    public string? BackendRegion { get; set; }

    [CliOption("--environment-name")]
    public string? EnvironmentName { get; set; }
}