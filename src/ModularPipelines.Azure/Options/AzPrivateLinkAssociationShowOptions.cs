using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("private-link", "association", "show")]
public record AzPrivateLinkAssociationShowOptions(
[property: CommandSwitch("--management-group-id")] string ManagementGroupId,
[property: CommandSwitch("--name")] string Name
) : AzOptions;