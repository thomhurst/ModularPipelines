using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("private-link", "association", "list")]
public record AzPrivateLinkAssociationListOptions(
[property: CliOption("--management-group-id")] string ManagementGroupId
) : AzOptions;