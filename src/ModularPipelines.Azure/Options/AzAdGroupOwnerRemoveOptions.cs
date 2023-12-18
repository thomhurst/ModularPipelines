using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ad", "group", "owner", "remove")]
public record AzAdGroupOwnerRemoveOptions(
[property: CommandSwitch("--group")] string Group,
[property: CommandSwitch("--owner-object-id")] string OwnerObjectId
) : AzOptions
{
}