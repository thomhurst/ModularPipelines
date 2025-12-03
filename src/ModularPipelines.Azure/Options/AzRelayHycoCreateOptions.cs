using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("relay", "hyco", "create")]
public record AzRelayHycoCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--requires-client-authorization")]
    public bool? RequiresClientAuthorization { get; set; }

    [CliOption("--user-metadata")]
    public string? UserMetadata { get; set; }
}