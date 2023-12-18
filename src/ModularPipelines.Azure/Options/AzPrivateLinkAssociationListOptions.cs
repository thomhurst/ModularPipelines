using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("private-link", "association", "list")]
public record AzPrivateLinkAssociationListOptions(
[property: CommandSwitch("--management-group-id")] string ManagementGroupId
) : AzOptions
{
}