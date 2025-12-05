using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("search", "private-endpoint-connection", "delete")]
public record AzSearchPrivateEndpointConnectionDeleteOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--service-name")] string ServiceName
) : AzOptions
{
    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}