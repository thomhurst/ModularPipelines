using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("private-link", "association", "show")]
public record AzPrivateLinkAssociationShowOptions(
[property: CliOption("--management-group-id")] string ManagementGroupId,
[property: CliOption("--name")] string Name
) : AzOptions;