using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("private-link", "association", "delete")]
public record AzPrivateLinkAssociationDeleteOptions(
[property: CliOption("--management-group-id")] string ManagementGroupId,
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}